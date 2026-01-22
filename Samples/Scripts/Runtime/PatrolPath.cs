using System;
using System.Collections.Generic;
using AV.Waypoints.Runtime;
using UnityEngine;

namespace AV.Waypoints.Samples.Runtime
{
    /// <summary>
    /// Demonstration component showing various WaypointAttribute configurations.
    /// This script follows strict naming guidelines: descriptive names, no abbreviations.
    /// </summary>
    public class PatrolPath : MonoBehaviour
    {
        [Header("Single Waypoints")]

        [Tooltip("Starting position for spawning entities")]
        [Waypoint]
        public Vector3 spawnPoint = Vector3.zero;

        [Tooltip("Exit point when patrol is complete")]
        [Waypoint(1f, 0f, 0f)] // Red color
        public Vector3 exitPoint = Vector3.forward * 10f;

        [Header("Multiple Waypoints")]

        [Tooltip("Array of possible exit destinations")]
        [Waypoint(0f, 1f, 0f)] // Green color
        public Vector3[] exitPoints = Array.Empty<Vector3>();

        [Tooltip("Sequential patrol route for AI characters")]
        [Waypoint(1f, 0.5f, 0f)] // Orange color
        public List<Vector3> patrolRoute = new();

        [Header("Debug Visualization")]

        [Tooltip("Show waypoint connections in Scene view")]
        public bool showConnections = true;

        [Tooltip("Label size for waypoint handles")]
        [Range(0.5f, 2f)]
        public float handleSize = 1f;

        #region Public API

        /// <summary>
        /// Gets the total number of waypoints across all configured fields.
        /// </summary>
        public int TotalWaypointCount
        {
            get
            {
                var count = 2; // spawnPoint + exitPoint
                count += exitPoints != null ? exitPoints.Length : 0;
                count += patrolRoute != null ? patrolRoute.Count : 0;
                return count;
            }
        }

        /// <summary>
        /// Gets the next waypoint in the patrol sequence.
        /// </summary>
        public bool TryGetNextWaypoint(int currentIndex, out Vector3 nextWaypoint)
        {
            if (patrolRoute == null || patrolRoute.Count == 0)
            {
                nextWaypoint = Vector3.zero;
                return false;
            }

            var nextIndex = (currentIndex + 1) % patrolRoute.Count;
            nextWaypoint = patrolRoute[nextIndex];
            return true;
        }

        /// <summary>
        /// Gets the closest exit point from a given position.
        /// </summary>
        public bool TryGetClosestExitPoint(Vector3 fromPosition, out Vector3 exitPoint)
        {
            exitPoint = Vector3.zero;

            if (exitPoints == null || exitPoints.Length == 0)
            {
                return false;
            }

            var closestDistance = float.MaxValue;
            var closestIndex = 0;

            for (var index = 0; index < exitPoints.Length; index++)
            {
                var distance = Vector3.Distance(fromPosition, exitPoints[index]);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = index;
                }
            }

            exitPoint = exitPoints[closestIndex];
            return true;
        }

        /// <summary>
        /// Validates that all waypoints have been configured.
        /// </summary>
        public bool ValidateWaypoints(out string errorMessage)
        {
            errorMessage = string.Empty;

            if (patrolRoute == null || patrolRoute.Count == 0)
            {
                errorMessage = "Patrol route has no waypoints defined.";
                return false;
            }

            if (exitPoints == null || exitPoints.Length == 0)
            {
                errorMessage = "No exit points configured.";
                return false;
            }

            return true;
        }

        #endregion

        #region Unity Lifecycle

        private void OnDrawGizmos()
        {
            if (!showConnections) return;

            // Draw spawn point
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(spawnPoint, 0.5f);
            Gizmos.DrawLine(spawnPoint, transform.position);

            // Draw exit point
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(exitPoint, 0.5f);

            // Draw exit points array
            if (exitPoints != null && exitPoints.Length > 0)
            {
                Gizmos.color = Color.green;
                foreach (var exit in exitPoints)
                {
                    Gizmos.DrawWireSphere(exit, 0.3f);
                    Gizmos.DrawLine(exit, transform.position);
                }
            }

            // Draw patrol route
            if (patrolRoute != null && patrolRoute.Count > 0)
            {
                Gizmos.color = new Color(1f, 0.5f, 0f); // Orange

                for (var index = 0; index < patrolRoute.Count; index++)
                {
                    var currentWaypoint = patrolRoute[index];
                    var nextWaypoint = patrolRoute[(index + 1) % patrolRoute.Count];

                    Gizmos.DrawWireSphere(currentWaypoint, 0.4f);
                    Gizmos.DrawLine(currentWaypoint, nextWaypoint);

                    // Draw direction indicator
                    var direction = (nextWaypoint - currentWaypoint).normalized;
                    Gizmos.DrawRay(currentWaypoint, direction * 0.5f);
                }
            }
        }

        #endregion

        #region Editor Debugging

        /// <summary>
        /// Test method callable from Unity Editor context menu.
        /// </summary>
        [ContextMenu("Debug: Log Waypoint Count")]
        private void LogWaypointCount()
        {
            Debug.Log($"[PatrolPath] Total waypoints: {TotalWaypointCount}", this);
            Debug.Log($"[PatrolPath] Patrol route count: {patrolRoute?.Count ?? 0}", this);
            Debug.Log($"[PatrolPath] Exit points count: {exitPoints?.Length ?? 0}", this);
        }

        /// <summary>
        /// Test method to validate waypoint configuration.
        /// </summary>
        [ContextMenu("Debug: Validate Waypoints")]
        private void ValidateWaypointsDebug()
        {
            if (ValidateWaypoints(out var errorMessage))
            {
                Debug.Log($"<color=cyan>[PatrolPath] ✅ All waypoints valid!</color>", this);
            }
            else
            {
                Debug.LogWarning($"<color=orange>[PatrolPath] ⚠️ Validation failed: {errorMessage}</color>", this);
            }
        }

        /// <summary>
        /// Test method to print all patrol route waypoints.
        /// </summary>
        [ContextMenu("Debug: Print Patrol Route")]
        private void PrintPatrolRoute()
        {
            if (patrolRoute == null || patrolRoute.Count == 0)
            {
                Debug.LogWarning("[PatrolPath] No patrol route defined", this);
                return;
            }

            Debug.Log($"[PatrolPath] Patrol Route ({patrolRoute.Count} waypoints):", this);
            for (var index = 0; index < patrolRoute.Count; index++)
            {
                var waypoint = patrolRoute[index];
                Debug.Log($"  {index + 1}. {waypoint}", this);
            }
        }

        #endregion
    }
}
