#if UNITY_EDITOR
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace AV.Waypoints.Editor.Validation
{
    /// <summary>
    ///     Validates waypoint data for common issues.
    /// </summary>
    public static class WaypointValidator
    {
        public const float MIN_DISTANCE_WARNING = 0.01f;
        public const float MAX_REASONABLE_DISTANCE = 1000f;

        /// <summary>
        ///     Checks if waypoints are too close together.
        /// </summary>
        public static bool AreWaypointsTooClose(Vector3 a, Vector3 b, out float distance)
        {
            distance = Vector3.Distance(a, b);
            return distance < MIN_DISTANCE_WARNING;
        }

        /// <summary>
        ///     Checks if waypoint distance is unreasonably large.
        /// </summary>
        public static bool IsDistanceUnreasonable(Vector3 a, Vector3 b, out float distance)
        {
            distance = Vector3.Distance(a, b);
            return distance > MAX_REASONABLE_DISTANCE;
        }

        /// <summary>
        ///     Validates an array of waypoints for common issues.
        /// </summary>
        public static List<string> ValidateWaypointArray(Vector3[] waypoints, string fieldName)
        {
            var warnings = new List<string>();

            if (waypoints == null)
            {
                warnings.Add($"{fieldName}: Array is null");
                return warnings;
            }

            if (waypoints.Length == 0)
            {
                warnings.Add($"{fieldName}: Array is empty");
                return warnings;
            }

            for (var i = 0; i < waypoints.Length - 1; i++)
            {
                if (AreWaypointsTooClose(waypoints[i], waypoints[i + 1], out var dist))
                    warnings.Add($"{fieldName}[{i}] and [{i + 1}] are very close ({dist:F4} units)");

                if (IsDistanceUnreasonable(waypoints[i], waypoints[i + 1], out dist))
                    warnings.Add($"{fieldName}[{i}] and [{i + 1}] are very far apart ({dist:F2} units)");
            }

            var uniquePositions = new HashSet<Vector3>();
            for (var i = 0; i < waypoints.Length; i++)
                if (!uniquePositions.Add(waypoints[i]))
                    warnings.Add($"{fieldName}[{i}] has duplicate position: {waypoints[i]}");

            return warnings;
        }

        /// <summary>
        ///     Validates float3 waypoints by converting to Vector3.
        /// </summary>
        public static List<string> ValidateWaypointArray(float3[] waypoints, string fieldName)
        {
            if (waypoints == null) return new List<string> { $"{fieldName}: Array is null" };

            var converted = new Vector3[waypoints.Length];
            for (var i = 0; i < waypoints.Length; i++)
                converted[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);

            return ValidateWaypointArray(converted, fieldName);
        }

        /// <summary>
        ///     Checks if a position is within reasonable bounds.
        /// </summary>
        public static bool IsPositionReasonable(Vector3 position)
        {
            return Mathf.Abs(position.x) < MAX_REASONABLE_DISTANCE &&
                   Mathf.Abs(position.y) < MAX_REASONABLE_DISTANCE &&
                   Mathf.Abs(position.z) < MAX_REASONABLE_DISTANCE;
        }
    }
}
#endif