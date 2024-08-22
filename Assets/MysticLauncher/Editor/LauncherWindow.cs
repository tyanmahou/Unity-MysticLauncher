using UnityEngine;
using UnityEditor;

namespace Mystic
{
    /// <summary>
    /// ランチャー
    /// </summary>
    public class LauncherWindow : EditorWindow
    {
        [MenuItem("Window/Mystic Launcher %L")]
        public static void ShowWindow()
        {
            var window = GetWindow<LauncherWindow>("Launcher");
            var icon = EditorGUIUtility.IconContent("d_Profiler.UIDetails");
            icon.text = "Launcher";
            window.titleContent = icon;
        }

        void OnGUI()
        {
            using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition);
            _scrollPosition = scrollView.scrollPosition;
            var projSettings = LauncherProjectSettings.instance;
            if (projSettings == null)
            {
                return;
            }
            DrawProjectHeader(projSettings);
        }

        void DrawProjectHeader(LauncherProjectSettings projSettings)
        {
            // タイトル
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                if (projSettings.ProjectInfo.Icon.TryGetGUIContent(out var icon))
                {
                    Rect iconRect = GUILayoutUtility.GetRect(45, 45);
                    GUI.DrawTexture(iconRect, icon.image, ScaleMode.ScaleToFit);
                }
                GUIStyle customStyle = new GUIStyle(GUI.skin.label);
                customStyle.fontSize = 30;
                customStyle.alignment = icon != null ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
                GUILayout.Label(projSettings.ProjectInfo.ProjectName, customStyle);
            }
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                GUILayout.FlexibleSpace();
                var icon = EditorGUIUtility.IconContent("d__Popup");
                icon.text = "Project ";
                if (GUILayout.Button(icon))
                {
                    SettingsService.OpenProjectSettings(LauncherProjectSettingsProvider.SettingPath);
                }
                icon.text = "User ";
                if (GUILayout.Button(icon))
                {
                    SettingsService.OpenUserPreferences(LauncherPreferenceSettingsProvider.SettingPath);
                }
            }
            EditorGUIUtil.DrawSeparator();
        }
        Vector2 _scrollPosition;
    }

}
