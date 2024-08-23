using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

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
            var projSettings = LauncherProjectSettings.instance;
            if (projSettings == null)
            {
                return;
            }
            DrawProjectHeader(projSettings);

            // タブの表示
            List<ITabLayout> tabs = new(2 + projSettings.CustomTabs.Length);
            tabs.Add(projSettings.Portal);
            tabs.Add(_favoriteLayout);
            tabs.AddRange(projSettings.CustomTabs);

            _selectedTab = GUILayout.Toolbar(
                _selectedTab,
                tabs.Select(TabContent).ToArray(),
                EditorStyles.toolbarButton,
                GUI.ToolbarButtonSize.FitToContents
                );

            using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition);
            tabs[_selectedTab].OnGUI();
            _scrollPosition = scrollView.scrollPosition;

        }
        GUIContent TabContent(ITabLayout layout)
        {
            if (layout.Icon.TryGetGUIContent(out var content))
            {
                content.text = layout.Title;
                return content;
            }
            return new GUIContent(layout.Title);
        }
        void DrawProjectHeader(LauncherProjectSettings projSettings)
        {
            // タイトル
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                if (projSettings.ProjectInfo.ProjectName.Icon.TryGetGUIContent(out var icon))
                {
                    Rect iconRect = GUILayoutUtility.GetRect(45, 45);
                    GUI.DrawTexture(iconRect, icon.image, ScaleMode.ScaleToFit);
                }
                GUIStyle customStyle = new GUIStyle(GUI.skin.label);
                customStyle.fontSize = 30;
                customStyle.alignment = icon != null ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
                GUILayout.Label(projSettings.ProjectInfo.ProjectName.Text, customStyle);
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
        int _selectedTab;

        FavoriteLayout _favoriteLayout = new FavoriteLayout();
    }

}
