# AV Waypoints

Professional waypoint editor tooling for Unity that provides intuitive Scene view handles and inspector integration for editing waypoint data.

## Features

- 🎯 **Scene View Editing** - Edit waypoints directly in the Scene view with visual handles
- 🎨 **Customizable Colors** - Assign custom colors to different waypoint types
- 📊 **Multiple Data Types** - Supports both `Vector3` and `float3` (Unity.Mathematics)
- 📦 **Array & List Support** - Works with individual fields, arrays, and lists
- 🔗 **Visual Connections** - Dotted lines show connections between waypoints
- ⌨️ **Grid Snapping** - Hold `Ctrl` to snap waypoints to grid (0.5 unit increments)
- 🏷️ **Labels** - Clear labels in Scene view for easy identification
- ⚡ **Performance Optimized** - Cached reflection and efficient rendering

## Installation

### Via Unity Package Manager

1. Open **Window > Package Manager**
2. Click the **+** button and select **Add package from git URL**
3. Enter: `https://github.com/yourusername/av-waypoints.git`

### Via Manifest.json

Add to your `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.av.waypoints": "https://github.com/yourusername/av-waypoints.git"
  }
}
```

## Quick Start

### 1. Add the Waypoint Attribute

```csharp
using AV.Waypoints.Runtime;
using Unity.Mathematics;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    // Default yellow color
    [Waypoint]
    public Vector3 spawnPoint;
    
    // Custom green color (RGB 0-1 range)
    [Waypoint(0f, 1f, 0f)]
    public float3 exitPoint;
    
    // Arrays are automatically connected with lines
    [Waypoint(1f, 0f, 1f)]
    public List<Vector3> patrolRoute = new List<Vector3>();
}
```

### 2. Edit in Scene View

1. Select your GameObject in the hierarchy
2. Click the **Waypoint Elite Tool** icon in the Scene view toolbar
3. Use the position handles to move waypoints
4. Hold **Ctrl** while dragging to snap to grid

### 3. Edit in Inspector

The Inspector shows Vector3 fields for precise numeric input. Changes sync automatically with Scene view.

## API Reference

### WaypointAttribute

Marks a field for waypoint editing in the Scene view.

```csharp
[AttributeUsage(AttributeTargets.Field)]
public class WaypointAttribute : PropertyAttribute
```

**Constructors:**
- `WaypointAttribute()` - Default yellow color (1f, 0.92f, 0.016f)
- `WaypointAttribute(float r, float g, float b)` - Custom RGB color (0-1 range)

**Supported Field Types:**
- `UnityEngine.Vector3`
- `Unity.Mathematics.float3`
- `Vector3[]` / `float3[]`
- `List<Vector3>` / `List<float3>`

## Color Presets

```csharp
// Yellow (default)
[Waypoint] public Vector3 point;

// Green
[Waypoint(0f, 1f, 0f)] public Vector3 point;

// Red
[Waypoint(1f, 0f, 0f)] public Vector3 point;

// Blue
[Waypoint(0f, 0f, 1f)] public Vector3 point;

// Cyan
[Waypoint(0f, 1f, 1f)] public Vector3 point;

// Magenta
[Waypoint(1f, 0f, 1f)] public Vector3 point;
```

## Keyboard Shortcuts

| Key | Action |
|-----|--------|
| `Ctrl` + Drag | Snap to 0.5 unit grid |

## Tips & Best Practices

1. **Organization** - Use different colors for different waypoint purposes (spawn points, patrol routes, exit points)
2. **Lists for Paths** - Use `List<Vector3>` or `Vector3[]` for connected waypoint sequences
3. **Local vs Global** - All waypoint positions are stored in local space relative to the GameObject's transform
4. **Pivot Mode** - Toggle between Local and Global rotation in the Scene view for different handle orientations

## Requirements

- **Unity:** 2021.3 or later
- **Dependencies:**
  - Unity.Mathematics (1.2.6+)

## Samples

Import the **Basic Waypoint Usage** sample from the Package Manager to see example implementations.

## Troubleshooting

### Handles not appearing?
- Ensure the **Waypoint Elite Tool** is selected in the Scene view toolbar
- Verify your GameObject has the MonoBehaviour component attached
- Check that fields have the `[Waypoint]` attribute

### Positions not saving?
- Make sure to apply changes in Play mode if testing at runtime
- Check that your scene is saved after editing

## License

[Your License Here]

## Support

For issues, questions, or contributions, please visit:
- GitHub: [Repository URL]
- Issues: [Issues URL]

---

Made with ❤️ by AV
