#if UNITY_EDITOR
using Unity.Mathematics;
using UnityEngine;

namespace AV.Waypoints.Editor.Utils
{
    /// <summary>
    ///     Utility extensions for waypoint operations.
    /// </summary>
    public static class WaypointExtensions
    {
        /// <summary>
        ///     Calculates total distance along a path of waypoints.
        /// </summary>
        public static float CalculatePathDistance(Vector3[] waypoints)
        {
            if (waypoints == null || waypoints.Length < 2) return 0f;

            var totalDistance = 0f;
            for (var i = 0; i < waypoints.Length - 1; i++)
                totalDistance += Vector3.Distance(waypoints[i], waypoints[i + 1]);
            return totalDistance;
        }

        /// <summary>
        ///     Calculates total distance along a path of float3 waypoints.
        /// </summary>
        public static float CalculatePathDistance(float3[] waypoints)
        {
            if (waypoints == null || waypoints.Length < 2) return 0f;

            var totalDistance = 0f;
            for (var i = 0; i < waypoints.Length - 1; i++)
                totalDistance += math.distance(waypoints[i], waypoints[i + 1]);
            return totalDistance;
        }

        /// <summary>
        ///     Gets approximate position at a given percentage along the path.
        /// </summary>
        public static Vector3 GetPositionAtPercentage(Vector3[] waypoints, float percentage)
        {
            if (waypoints == null || waypoints.Length == 0) return Vector3.zero;
            if (waypoints.Length == 1) return waypoints[0];

            percentage = Mathf.Clamp01(percentage);
            var totalDistance = CalculatePathDistance(waypoints);
            var targetDistance = totalDistance * percentage;

            var accumulatedDistance = 0f;
            for (var i = 0; i < waypoints.Length - 1; i++)
            {
                var segmentDistance = Vector3.Distance(waypoints[i], waypoints[i + 1]);

                if (accumulatedDistance + segmentDistance >= targetDistance)
                {
                    var t = (targetDistance - accumulatedDistance) / segmentDistance;
                    return Vector3.Lerp(waypoints[i], waypoints[i + 1], t);
                }

                accumulatedDistance += segmentDistance;
            }

            return waypoints[waypoints.Length - 1];
        }

        /// <summary>
        ///     Converts Vector3 to float3.
        /// </summary>
        public static float3 ToFloat3(this Vector3 v)
        {
            return new float3(v.x, v.y, v.z);
        }

        /// <summary>
        ///     Converts float3 to Vector3.
        /// </summary>
        public static Vector3 ToVector3(this float3 f)
        {
            return new Vector3(f.x, f.y, f.z);
        }
    }
}
#endif