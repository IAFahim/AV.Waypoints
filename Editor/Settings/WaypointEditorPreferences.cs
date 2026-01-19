#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[HelpURL("https://github.com/IAFahim/AV.Waypoints")]

namespace AV.Waypoints.Editor.Settings
{
    /// <summary>
    ///     Editor preferences for waypoint tool settings.
    /// </summary>
    public static class WaypointEditorPreferences
    {
        private const string PREF_SNAP_ENABLED = "AVWaypoints_SnapEnabled";
        private const string PREF_SNAP_SIZE = "AVWaypoints_SnapSize";
        private const string PREF_SHOW_LABELS = "AVWaypoints_ShowLabels";
        private const string PREF_HANDLE_SIZE = "AVWaypoints_HandleSize";
        private const string PREF_LINE_THICKNESS = "AVWaypoints_LineThickness";

        public static bool SnapEnabled
        {
            get => EditorPrefs.GetBool(PREF_SNAP_ENABLED, false);
            set => EditorPrefs.SetBool(PREF_SNAP_ENABLED, value);
        }

        public static float SnapSize
        {
            get => EditorPrefs.GetFloat(PREF_SNAP_SIZE, 0.5f);
            set => EditorPrefs.SetFloat(PREF_SNAP_SIZE, Mathf.Max(0.01f, value));
        }

        public static bool ShowLabels
        {
            get => EditorPrefs.GetBool(PREF_SHOW_LABELS, true);
            set => EditorPrefs.SetBool(PREF_SHOW_LABELS, value);
        }

        public static float HandleSize
        {
            get => EditorPrefs.GetFloat(PREF_HANDLE_SIZE, 1.0f);
            set => EditorPrefs.SetFloat(PREF_HANDLE_SIZE, Mathf.Clamp(value, 0.1f, 3.0f));
        }

        public static float LineThickness
        {
            get => EditorPrefs.GetFloat(PREF_LINE_THICKNESS, 4.0f);
            set => EditorPrefs.SetFloat(PREF_LINE_THICKNESS, Mathf.Clamp(value, 1.0f, 10.0f));
        }
    }

    /// <summary>
    ///     Settings provider for Waypoint preferences in Unity Preferences window.
    /// </summary>
    public class WaypointPreferencesProvider : SettingsProvider
    {
        public WaypointPreferencesProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope)
        {
        }

        public override void OnGUI(string searchContext)
        {
            EditorGUILayout.LabelField("Waypoint Tool Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            var snapEnabled = EditorGUILayout.Toggle(new GUIContent("Enable Auto Snap",
                    "Automatically snap waypoints to grid (without holding Ctrl)"),
                WaypointEditorPreferences.SnapEnabled);

            var snapSize = EditorGUILayout.FloatField(new GUIContent("Snap Grid Size",
                    "Grid increment for snapping (in units)"),
                WaypointEditorPreferences.SnapSize);

            EditorGUILayout.Space(10);

            var showLabels = EditorGUILayout.Toggle(new GUIContent("Show Labels",
                    "Display waypoint labels in Scene view"),
                WaypointEditorPreferences.ShowLabels);

            var handleSize = EditorGUILayout.Slider(new GUIContent("Handle Size",
                    "Scale multiplier for position handles"),
                WaypointEditorPreferences.HandleSize, 0.1f, 3.0f);

            var lineThickness = EditorGUILayout.Slider(new GUIContent("Connection Line Thickness",
                    "Dotted line thickness between waypoints"),
                WaypointEditorPreferences.LineThickness, 1.0f, 10.0f);

            if (EditorGUI.EndChangeCheck())
            {
                WaypointEditorPreferences.SnapEnabled = snapEnabled;
                WaypointEditorPreferences.SnapSize = snapSize;
                WaypointEditorPreferences.ShowLabels = showLabels;
                WaypointEditorPreferences.HandleSize = handleSize;
                WaypointEditorPreferences.LineThickness = lineThickness;
            }

            EditorGUILayout.Space(15);
            EditorGUILayout.HelpBox("Hold Ctrl while dragging to temporarily toggle grid snapping.", MessageType.Info);
        }

        [SettingsProvider]
        public static SettingsProvider CreateWaypointPreferencesProvider()
        {
            return new WaypointPreferencesProvider("Preferences/AV Waypoints")
            {
                keywords = new[] { "waypoint", "editor", "tool", "snap", "grid", "handle" }
            };
        }
    }
}
#endif