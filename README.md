# AV.Waypoints

![Header](documentation_header.svg)

[![Unity](https://img.shields.io/badge/Unity-2022.3%2B-000000.svg?style=flat-square&logo=unity)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](LICENSE.md)

Professional Scene View waypoint editor tooling.

## ✨ Features

- **[Waypoint] Attribute**: Marks `Vector3` or `float3` fields for scene editing.
- **Visual Handles**: Move points directly in the Scene view.
- **Arrays/Lists**: Supports collections of waypoints with connecting lines.
- **Grid Snapping**: Hold Ctrl to snap points.

## 📦 Installation

Install via Unity Package Manager (git URL).

### Dependencies
- **Unity.Mathematics**

## 🚀 Usage

```csharp
using AV.Waypoints.Runtime;

public class Patrol : MonoBehaviour
{
    [Waypoint] public Vector3 spawnPoint;
    [Waypoint(1, 0, 0)] public List<Vector3> route; // Red path
}
```

## ⚠️ Status

- 🧪 **Tests**: Missing.
- 📘 **Samples**: Included in `Samples~`.
