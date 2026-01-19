#if UNITY_EDITOR
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace AV.Waypoints.Editor.Infrastructure
{
    /// <summary>
    /// Bridges the gap between Unity's generic SerializedProperty and our logic's float3.
    /// Extracts and applies position data for both Vector3 and float3 property types.
    /// </summary>
    public static class WaypointSerializationAdapter
    {
        /// <summary>
        /// Extracts position data from a SerializedProperty into float3 format.
        /// Handles both Vector3 and float3 property types.
        /// </summary>
        public static void ExtractPositionFromProperty(SerializedProperty property, out float3 position)
        {
            position = float3.zero;

            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                Vector3 vectorValue = property.vector3Value;
                position = new float3(vectorValue.x, vectorValue.y, vectorValue.z);
                return;
            }

            // Handle float3 generic types
            if (property.propertyType == SerializedPropertyType.Generic)
            {
                SerializedProperty x = property.FindPropertyRelative("x");
                SerializedProperty y = property.FindPropertyRelative("y");
                SerializedProperty z = property.FindPropertyRelative("z");

                if (x != null && y != null && z != null)
                {
                    position = new float3(x.floatValue, y.floatValue, z.floatValue);
                }
            }
        }

        /// <summary>
        /// Applies a float3 position to a SerializedProperty.
        /// Handles both Vector3 and float3 property types.
        /// </summary>
        public static void ApplyPositionToProperty(SerializedProperty property, in float3 newPosition)
        {
            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                property.vector3Value = new Vector3(newPosition.x, newPosition.y, newPosition.z);
                return;
            }

            if (property.propertyType == SerializedPropertyType.Generic)
            {
                SerializedProperty x = property.FindPropertyRelative("x");
                SerializedProperty y = property.FindPropertyRelative("y");
                SerializedProperty z = property.FindPropertyRelative("z");

                if (x != null && y != null && z != null)
                {
                    x.floatValue = newPosition.x;
                    y.floatValue = newPosition.y;
                    z.floatValue = newPosition.z;
                }
            }
        }
    }
}
#endif
