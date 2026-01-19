using System;
using UnityEngine;

namespace AV.Waypoints.Runtime
{
    /// <summary>
    /// Marks a Vector3 or float3 field for waypoint editing in the Scene view.
    /// Supports individual fields, arrays, and lists.
    /// </summary>
    [HelpURL("https://github.com/IAFahim/AV.Waypoints")]
    [AttributeUsage(AttributeTargets.Field)]
    public class WaypointAttribute : PropertyAttribute
    {
        public readonly float RedComponent;
        public readonly float GreenComponent;
        public readonly float BlueComponent;
        public readonly bool UseCustomColor;

        public WaypointAttribute()
        {
            UseCustomColor = false;
            RedComponent = 1f;
            GreenComponent = 0.92f;
            BlueComponent = 0.016f;
        }

        public WaypointAttribute(float red, float green, float blue)
        {
            RedComponent = red;
            GreenComponent = green;
            BlueComponent = blue;
            UseCustomColor = true;
        }
    }
}