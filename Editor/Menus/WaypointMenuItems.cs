#if UNITY_EDITOR
using AV.Waypoints.Editor.Tools;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;

namespace AV.Waypoints.Editor.Menus
{
    /// <summary>
    ///     Menu items for quick access to waypoint tools and documentation.
    /// </summary>
    public static class WaypointMenuItems
    {
        [MenuItem("Tools/AV Waypoints/Open Preferences", false, 1)]
        private static void OpenPreferences()
        {
            SettingsService.OpenUserPreferences("Preferences/AV Waypoints");
        }

        [MenuItem("Tools/AV Waypoints/Documentation", false, 2)]
        private static void OpenDocumentation()
        {
            var readmePath = AssetDatabase.FindAssets("README t:TextAsset",
                new[] { "Packages/com.av.waypoints", "Assets/AV.Waypoints" });

            if (readmePath.Length > 0)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(readmePath[0]);
                var readme = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
                if (readme != null)
                {
                    EditorUtility.OpenWithDefaultApp(assetPath);
                    return;
                }
            }

            EditorUtility.DisplayDialog("Documentation",
                "README.md not found. Check the package documentation in the Package Manager.",
                "OK");
        }

        [MenuItem("Tools/AV Waypoints/Select Waypoint Tool", false, 20)]
        private static void SelectWaypointTool()
        {
            ToolManager.SetActiveTool(typeof(WaypointEditorTool));
        }

        [MenuItem("Tools/AV Waypoints/About", false, 100)]
        private static void ShowAbout()
        {
            EditorUtility.DisplayDialog(
                "AV Waypoints v1.0.0",
                "Professional waypoint editor tooling for Unity.\n\n" +
                "Features:\n" +
                "• Scene view editing with handles\n" +
                "• Vector3 & float3 support\n" +
                "• Array & List support\n" +
                "• Custom colors per field\n" +
                "• Grid snapping\n" +
                "• Visual connections\n\n" +
                "© 2026 AV - MIT License",
                "OK"
            );
        }
    }
}
#endif