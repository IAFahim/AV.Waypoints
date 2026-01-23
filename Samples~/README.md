# AV Waypoints Samples

This folder contains example scenes and scripts demonstrating various waypoint configurations.

## Contents

- **AV.Waypoint.unity** - Example scene with a PatrolPath component configured
- **Scripts/Runtime/PatrolPath.cs** - Test script demonstrating waypoint attribute usage

## Usage

1. Open the sample scene
2. Select the PatrolPath GameObject
3. Click the Waypoint Elite Tool in the Scene view toolbar
4. Edit waypoints by dragging handles
5. Observe visual connections between waypoints

## PatrolPath Component

The PatrolPath script demonstrates:
- **Single waypoint fields** (SpawnPoint, ExitPoint)
- **Array of waypoints** (ExitPoints)
- **List of waypoints** (PatrolRoute)
- **Custom colors per field type** (Yellow, Red, Green, Orange)
- **Editor debugging** with Context Menu commands
- **Gizmo visualization** in Scene view
- **Public API** for waypoint queries

### Features

```csharp
// Get total waypoint count across all fields
int count = patrolPath.TotalWaypointCount;

// Get next waypoint in sequence
if (patrolPath.TryGetNextWaypoint(currentIndex, out Vector3 next))
{
    // Move to next waypoint
}

// Find closest exit point
if (patrolPath.TryGetClosestExitPoint(transform.position, out Vector3 exit))
{
    // Use exit point
}

// Validate waypoint configuration
if (patrolPath.ValidateWaypoints(out string error))
{
    // All waypoints valid
}
```

### Context Menu Commands

Right-click the PatrolPath component in Inspector to access debug tools:

- **Debug: Log Waypoint Count** - Prints waypoint statistics
- **Debug: Validate Waypoints** - Checks configuration validity
- **Debug: Print Patrol Route** - Lists all patrol waypoints

### Code Quality

The PatrolPath component follows strict naming guidelines from AGENTS.md:
- ✅ **Descriptive names**: `spawnPoint` (not `spawn`)
- ✅ **Full words**: `exitPoints` (not `exits` or `exitPts`)
- ✅ **Clear intent**: `TryGetClosestExitPoint` (not `GetClosest`)
- ✅ **Pronounceable**: All names read naturally
- ✅ **No abbreviations**: `patrolRoute` (not `route` or `path`)

Feel free to duplicate and modify these examples for your own use cases!
