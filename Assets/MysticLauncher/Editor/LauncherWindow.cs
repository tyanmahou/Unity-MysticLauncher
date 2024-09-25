using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

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
            var icon = new GUIContent(EditorGUIUtility.IconContent("d_Profiler.UIDetails"));
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
            var userSettings = LauncherUserSettings.instance;
            if (userSettings == null)
            {
                return;
            }
            DrawProjectHeader(projSettings);

            List<ITabLayout> tabs = new(1 + projSettings.ProjectTabs.Length + userSettings.UserTabs.Length)
                {
                    new PortalLayout(),
                };
            tabs.AddRange(projSettings.ProjectTabs.Where(t => t != null));
            tabs.AddRange(userSettings.UserTabs.Where(t => t != null));

            // タブの表示
            using (var tabScroller = new GUILayout.ScrollViewScope(_tabScrollPosition, GUILayout.ExpandHeight(false)))
            {
                _selectedTab = GUILayout.Toolbar(
                    _selectedTab,
                    tabs.Select(TabContent).ToArray(),
                    EditorStyles.toolbarButton,
                    GUI.ToolbarButtonSize.FitToContents
                    );

                _tabScrollPosition = tabScroller.scrollPosition;
            }

            // コンテンツの表示
            using (var contentScroller = new GUILayout.ScrollViewScope(_contentScrollPosition))
            {
                if (_selectedTab < tabs.Count)
                {
                    tabs[_selectedTab].OnGUI();
                }
                _contentScrollPosition = contentScroller.scrollPosition;
            }
        }
        GUIContent TabContent(ITabLayout layout)
        {
            if (layout.Icon.TryGetGUIContent(out var content))
            {
                content.text = layout.Title;
                if (!_iconTextures.TryGetValue(content.image, out Texture icon))
                {
                    icon = EditorGUIUtil.ResizeTexture(content.image, 16, 16);
                    _iconTextures.Add(content.image, icon);
                }
                content.image = icon;
                return content;
            }
            return new GUIContent(layout.Title);
        }
       
        void DrawProjectHeader(LauncherProjectSettings projSettings)
        {
            if (!string.IsNullOrEmpty(projSettings.ProjectInfo.HelpUrl))
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), EditorStyles.iconButton))
                {
                    using Process process = System.Diagnostics.Process.Start(projSettings.ProjectInfo.HelpUrl);
                }
                GUILayout.Space(4);
            }

            // タイトル
            if (projSettings.ProjectInfo.CustomHeader == null) {
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
            else
            {
                projSettings.ProjectInfo.CustomHeader.OnGUI();
            }
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                GUILayout.FlexibleSpace();
                if (EditorGUIUtil.IconTextButton("d__Popup", "Project "))
                {
                    SettingsService.OpenProjectSettings(LauncherProjectSettingsProvider.SettingPath);
                }
                if (EditorGUIUtil.IconTextButton("d__Popup", "User "))
                {
                    SettingsService.OpenUserPreferences(LauncherUserSettingsProvider.SettingPath);
                }
            }
            EditorGUIUtil.DrawSeparator();
        }
        Vector2 _tabScrollPosition;
        Vector2 _contentScrollPosition;
        int _selectedTab;

        static Dictionary<Texture, Texture> _iconTextures = new();
    }
}
