#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using AV.Waypoints.Runtime;

namespace AV.Waypoints.Editor.Infrastructure
{
    /// <summary>
    /// Caches reflection data for types with [Waypoint] attributes.
    /// Prevents repeated reflection lookups during editor operations.
    /// </summary>
    public static class WaypointFieldCache
    {
        private static readonly Dictionary<Type, List<string>> WaypointFieldsByType = new Dictionary<Type, List<string>>();

        /// <summary>
        /// Gets all field names that have the [Waypoint] attribute for a given type.
        /// Results are cached for performance.
        /// </summary>
        public static List<string> GetFieldNamesWithAttribute(Type typeToInspect)
        {
            if (WaypointFieldsByType.TryGetValue(typeToInspect, out List<string> cachedPaths))
            {
                return cachedPaths;
            }

            var newPaths = new List<string>();
            var fields = typeToInspect.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                if (Attribute.IsDefined(field, typeof(WaypointAttribute)))
                {
                    newPaths.Add(field.Name);
                }
            }

            WaypointFieldsByType[typeToInspect] = newPaths;
            return newPaths;
        }
    }
}
#endif
