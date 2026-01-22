#if UNITY_EDITOR
using AV.Waypoints.Editor.Infrastructure;
using AV.Waypoints.Runtime;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace AV.Waypoints.Editor
{
    /// <summary>
    /// Custom property drawer for [Waypoint] attribute.
    /// Provides Vector3 field display in the Inspector.
    /// </summary>
    [CustomPropertyDrawer(typeof(WaypointAttribute))]
    public class WaypointAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            WaypointSerializationAdapter.ExtractPositionFromProperty(property, out var currentPos);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();

            var newVec = EditorGUI.Vector3Field(position, label,
                new Vector3(currentPos.x, currentPos.y, currentPos.z));

            if (EditorGUI.EndChangeCheck())
            {
                var newPos = new float3(newVec.x, newVec.y, newVec.z);
                WaypointSerializationAdapter.ApplyPositionToProperty(property, in newPos);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
#endif