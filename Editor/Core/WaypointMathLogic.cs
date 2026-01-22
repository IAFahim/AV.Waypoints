#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;

namespace AV.Waypoints.Editor.Core
{
    /// <summary>
    /// Stateless, Burst-compatible logic layer for waypoint coordinate transformations and snapping.
    /// Follows DOD principles with in/out pattern for all operations.
    /// </summary>
    [BurstCompile]
    public static class WaypointMathLogic
    {
        public const float SnapThreshold = 0.001f;
        public const float Epsilon = 0.000001f;

        /// <summary>
        /// Transforms a position from local space to world space using the provided matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformToWorldCoordinates(in float3 localPosition, in float4x4 localToWorldMatrix, out float3 worldPosition)
        {
            worldPosition = math.transform(localToWorldMatrix, localPosition);
        }

        /// <summary>
        /// Transforms a position from world space to local space using the provided matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TransformToLocalCoordinates(in float3 worldPosition, in float4x4 worldToLocalMatrix, out float3 localPosition)
        {
            localPosition = math.transform(worldToLocalMatrix, worldPosition);
        }

        /// <summary>
        /// Snaps a position to the nearest grid increment if the grid size is valid.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CalculateSnappedPosition(in float3 inputPosition, in float gridSize, out float3 resultPosition)
        {
            if (gridSize <= SnapThreshold)
            {
                resultPosition = inputPosition;
                return;
            }

            // DOD: Explicit calculation preferable to LINQ or object wrappers
            float x = math.round(inputPosition.x / gridSize) * gridSize;
            float y = math.round(inputPosition.y / gridSize) * gridSize;
            float z = math.round(inputPosition.z / gridSize) * gridSize;

            resultPosition = new float3(x, y, z);
        }

        /// <summary>
        /// Determines if two positions are effectively the same within a small error margin.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsApproximatelyEqual(in float3 positionA, in float3 positionB, out bool areEqual)
        {
            areEqual = math.distancesq(positionA, positionB) < Epsilon;
        }

        /// <summary>
        /// Interpolates between two points for visualization logic.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InterpolatePosition(in float3 start, in float3 end, in float t, out float3 result)
        {
            result = math.lerp(start, end, t);
        }
    }
}
#endif
