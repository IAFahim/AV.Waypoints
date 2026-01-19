#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using AV.Waypoints.Editor.Core;
using AV.Waypoints.Editor.Infrastructure;
using AV.Waypoints.Editor.Settings;
using AV.Waypoints.Runtime;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace AV.Waypoints.Editor.Tools
{
    /// <summary>
    /// Unity Editor Tool for visually editing waypoints in the Scene view.
    /// Orchestrates data fetching, logic calls, and rendering using the Data-Logic-Adapter pattern.
    /// </summary>
    [HelpURL("https://github.com/IAFahim/AV.Waypoints")]
    [AddComponentMenu("AV/Waypoints/WaypointEditorTool")]
    [EditorTool("Waypoint Elite Tool")]
    public class WaypointEditorTool : EditorTool
    {
        [SerializeField] private Texture2D m_ToolIcon;
        private GUIContent m_ToolbarContent;

        private SerializedObject m_SerializedTargetObject;
        private Type m_CachedTargetType;
        private List<string> m_CachedFieldPaths;

        public override GUIContent toolbarIcon
        {
            get
            {
                if (m_ToolbarContent == null)
                {
                    m_ToolbarContent = new GUIContent(
                        m_ToolIcon != null ? m_ToolIcon : EditorGUIUtility.IconContent("MoveTool").image,
                        "Waypoint Elite Editor");
                }
                return m_ToolbarContent;
            }
        }

        public override bool IsAvailable()
        {
            if (Selection.activeGameObject == null)
                return false;

            var activeMonoBehaviour = Selection.activeGameObject.GetComponent<MonoBehaviour>();
            if (activeMonoBehaviour == null || !activeMonoBehaviour.enabled)
                return false;

            var fields = WaypointFieldCache.GetFieldNamesWithAttribute(activeMonoBehaviour.GetType());
            return fields != null && fields.Count > 0;
        }

        public override void OnToolGUI(EditorWindow window)
        {
            if (!(target is MonoBehaviour targetMonoBehaviour))
                return;

            if (Selection.activeGameObject != targetMonoBehaviour.gameObject)
                return;

            if (ShouldRebuildCache(targetMonoBehaviour))
            {
                RebuildCache(targetMonoBehaviour);
            }

            if (m_CachedFieldPaths == null || m_CachedFieldPaths.Count == 0)
            {
                if (ToolManager.activeToolType == typeof(WaypointEditorTool))
                {
                    ToolManager.SetActiveTool((Type)null);
                }
                return;
            }

            m_SerializedTargetObject.Update();

            bool propertiesModified = false;
            Transform targetTransform = targetMonoBehaviour.transform;

            // Extract matrix data for Logic layer
            float4x4 localToWorldMatrix = targetTransform.localToWorldMatrix;
            float4x4 worldToLocalMatrix = targetTransform.worldToLocalMatrix;
            Quaternion activeRotation = UnityEditor.Tools.pivotRotation == PivotRotation.Local
                ? targetTransform.rotation
                : Quaternion.identity;

            foreach (string fieldPath in m_CachedFieldPaths)
            {
                propertiesModified |= ProcessField(
                    fieldPath,
                    targetTransform.position,
                    activeRotation,
                    in localToWorldMatrix,
                    in worldToLocalMatrix
                );
            }

            if (propertiesModified)
            {
                m_SerializedTargetObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(targetMonoBehaviour);
            }
        }

        private bool ShouldRebuildCache(MonoBehaviour currentTarget)
        {
            return m_SerializedTargetObject == null || m_SerializedTargetObject.targetObject != currentTarget;
        }

        private void RebuildCache(MonoBehaviour currentTarget)
        {
            m_SerializedTargetObject = new SerializedObject(currentTarget);
            m_CachedTargetType = currentTarget.GetType();
            m_CachedFieldPaths = WaypointFieldCache.GetFieldNamesWithAttribute(m_CachedTargetType);
        }

        private bool ProcessField(
            string fieldPath,
            Vector3 anchorPosition,
            Quaternion handleRotation,
            in float4x4 localToWorldMatrix,
            in float4x4 worldToLocalMatrix)
        {
            SerializedProperty property = m_SerializedTargetObject.FindProperty(fieldPath);
            if (property == null) return false;

            FieldInfo fieldInfo = m_CachedTargetType.GetField(fieldPath, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var attribute = (WaypointAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(WaypointAttribute));

            Color handleColor = attribute.UseCustomColor
                ? new Color(attribute.RedComponent, attribute.GreenComponent, attribute.BlueComponent)
                : Color.cyan;

            if (property.isArray && property.propertyType != SerializedPropertyType.String)
            {
                return ProcessArrayProperty(property, anchorPosition, handleRotation, handleColor, in localToWorldMatrix, in worldToLocalMatrix);
            }

            return ProcessSingleProperty(property, property.displayName, anchorPosition, handleRotation, handleColor, in localToWorldMatrix, in worldToLocalMatrix);
        }

        private bool ProcessArrayProperty(
            SerializedProperty listProperty,
            Vector3 anchorPosition,
            Quaternion handleRotation,
            Color color,
            in float4x4 localToWorldMatrix,
            in float4x4 worldToLocalMatrix)
        {
            bool hasChanges = false;
            float3? previousWorldPosition = null;

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
                string label = $"{listProperty.displayName}[{i}]";

                hasChanges |= ProcessSingleProperty(
                    elementProperty,
                    label,
                    anchorPosition,
                    handleRotation,
                    color,
                    in localToWorldMatrix,
                    in worldToLocalMatrix,
                    previousWorldPosition
                );

                // Update previous position for the next iteration's line drawing
                WaypointSerializationAdapter.ExtractPositionFromProperty(elementProperty, out float3 currentLocalPosition);
                WaypointMathLogic.TransformToWorldCoordinates(in currentLocalPosition, in localToWorldMatrix, out float3 currentWorldPosition);
                previousWorldPosition = currentWorldPosition;
            }

            return hasChanges;
        }

        private bool ProcessSingleProperty(
            SerializedProperty property,
            string label,
            Vector3 anchorPosition,
            Quaternion handleRotation,
            Color color,
            in float4x4 localToWorldMatrix,
            in float4x4 worldToLocalMatrix,
            float3? connectToLinePosition = null)
        {
            WaypointSerializationAdapter.ExtractPositionFromProperty(property, out float3 localPosition);
            WaypointMathLogic.TransformToWorldCoordinates(in localPosition, in localToWorldMatrix, out float3 worldPosition);

            // Draw Visuals
            DrawWaypointVisuals(worldPosition, anchorPosition, connectToLinePosition, color, label);

            // Handle Interaction
            EditorGUI.BeginChangeCheck();

            Vector3 newWorldPositionVector = Handles.PositionHandle(new Vector3(worldPosition.x, worldPosition.y, worldPosition.z), handleRotation);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_SerializedTargetObject.targetObject, "Move Waypoint");

                float3 newWorldPosition = new float3(newWorldPositionVector.x, newWorldPositionVector.y, newWorldPositionVector.z);

                // Logic: Convert back to local
                WaypointMathLogic.TransformToLocalCoordinates(in newWorldPosition, in worldToLocalMatrix, out float3 rawLocalPosition);

                // Logic: Snap
                bool isSnappingEnabled = WaypointEditorPreferences.SnapEnabled ? !Event.current.control : Event.current.control;
                if (isSnappingEnabled)
                {
                    WaypointMathLogic.CalculateSnappedPosition(in rawLocalPosition, WaypointEditorPreferences.SnapSize, out rawLocalPosition);
                }

                // Adapter: Write back
                WaypointSerializationAdapter.ApplyPositionToProperty(property, in rawLocalPosition);
                return true;
            }

            return false;
        }

        private void DrawWaypointVisuals(float3 worldPosition, Vector3 anchorPosition, float3? connectToLinePosition, Color color, string label)
        {
            Handles.color = color;

            // Draw Line
            Vector3 lineStart = connectToLinePosition.HasValue ? (Vector3)connectToLinePosition.Value : anchorPosition;
            Handles.DrawDottedLine(lineStart, (Vector3)worldPosition, WaypointEditorPreferences.LineThickness);

            // Draw Label if visible
            if (WaypointEditorPreferences.ShowLabels)
            {
                // Cull labels behind camera
                Vector3 cameraPosition = SceneView.currentDrawingSceneView.camera.transform.position;
                Vector3 cameraForward = SceneView.currentDrawingSceneView.camera.transform.forward;
                Vector3 directionToPoint = (Vector3)worldPosition - cameraPosition;

                if (Vector3.Dot(cameraForward, directionToPoint) > 0)
                {
                    DrawSafeLabel(worldPosition, label);
                }
            }
        }

        private void DrawSafeLabel(float3 position, string text)
        {
            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = Color.white },
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            Vector3 labelPos = position + new float3(0, 0.2f, 0);

            Handles.BeginGUI();
            // Project world point to screen
            Vector2 screenPoint = HandleUtility.WorldToGUIPoint(labelPos);
            var content = new GUIContent(text);
            var size = labelStyle.CalcSize(content);
            var rect = new Rect(screenPoint.x - size.x / 2, screenPoint.y - size.y / 2, size.x, size.y);

            // Shadow
            var shadowRect = rect;
            shadowRect.position += new Vector2(1f, 1f);
            var shadowStyle = new GUIStyle(labelStyle) { normal = { textColor = Color.black } };
            GUI.Label(shadowRect, content, shadowStyle);

            // Text
            GUI.Label(rect, content, labelStyle);
            Handles.EndGUI();
        }
    }
}
#endif
