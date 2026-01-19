#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using AV.Waypoints.Runtime;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace AV.Waypoints.Editor.Gizmos
{
    /// <summary>
    ///     Draws gizmos for waypoint fields even when the tool is not active.
    ///     Provides always-visible indicators in Scene view.
    /// </summary>
    [InitializeOnLoad]
    public static class WaypointGizmoDrawer
    {
        // Gizmo rendering settings
        private const float SphereHandleSize = 0.2f;
        private const float DottedLineSpacing = 3f;

        private static readonly Color GizmoColor = new(1f, 0.92f, 0.016f, 0.3f);
        private static readonly Color GizmoLineColor = new(1f, 0.92f, 0.016f, 0.15f);

        static WaypointGizmoDrawer()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (Selection.activeGameObject == null) return;

            var components = Selection.activeGameObject.GetComponents<MonoBehaviour>();
            if (components == null) return;

            foreach (var component in components)
            {
                if (component == null) continue;
                DrawWaypointsForComponent(component);
            }
        }

        private static void DrawWaypointsForComponent(MonoBehaviour component)
        {
            var type = component.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(WaypointAttribute))) continue;

                var attr = (WaypointAttribute)Attribute.GetCustomAttribute(field, typeof(WaypointAttribute));
                var color = attr.UseCustomColor ? new Color(attr.RedComponent, attr.GreenComponent, attr.BlueComponent, 0.3f) : GizmoColor;

                var value = field.GetValue(component);
                if (value == null) continue;

                var transform = component.transform;
                var matrix = transform.localToWorldMatrix;

                if (value is Vector3 v3)
                    DrawSingleWaypoint(v3, matrix, color);
                else if (value is float3 f3)
                    DrawSingleWaypoint(new Vector3(f3.x, f3.y, f3.z), matrix, color);
                else if (value is Vector3[] v3Array)
                    DrawWaypointArray(v3Array, matrix, color);
                else if (value is float3[] f3Array)
                    DrawWaypointArray(f3Array, matrix, color);
                else if (value is List<Vector3> v3List)
                    DrawWaypointArray(v3List.ToArray(), matrix, color);
                else if (value is List<float3> f3List) DrawWaypointArray(f3List.ToArray(), matrix, color);
            }
        }

        private static void DrawSingleWaypoint(Vector3 localPos, Matrix4x4 matrix, Color color)
        {
            var worldPos = matrix.MultiplyPoint3x4(localPos);

            Handles.color = color;
            Handles.SphereHandleCap(0, worldPos, Quaternion.identity, SphereHandleSize * HandleUtility.GetHandleSize(worldPos),
                EventType.Repaint);
        }

        private static void DrawWaypointArray(Vector3[] localPositions, Matrix4x4 matrix, Color color)
        {
            if (localPositions == null || localPositions.Length == 0) return;

            var previousWorld = Vector3.zero;

            for (var i = 0; i < localPositions.Length; i++)
            {
                var worldPos = matrix.MultiplyPoint3x4(localPositions[i]);

                Handles.color = color;
                Handles.SphereHandleCap(0, worldPos, Quaternion.identity, SphereHandleSize * HandleUtility.GetHandleSize(worldPos),
                    EventType.Repaint);

                if (i > 0)
                {
                    Handles.color = new Color(color.r, color.g, color.b, 0.15f);
                    Handles.DrawDottedLine(previousWorld, worldPos, DottedLineSpacing);
                }

                previousWorld = worldPos;
            }
        }

        private static void DrawWaypointArray(float3[] localPositions, Matrix4x4 matrix, Color color)
        {
            if (localPositions == null || localPositions.Length == 0) return;

            var converted = new Vector3[localPositions.Length];
            for (var i = 0; i < localPositions.Length; i++)
                converted[i] = new Vector3(localPositions[i].x, localPositions[i].y, localPositions[i].z);

            DrawWaypointArray(converted, matrix, color);
        }
    }
}
#endif