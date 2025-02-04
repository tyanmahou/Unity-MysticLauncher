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
            var icon = new GUIContent(EditorGUIUtility.IconContent("d_Profiler.UIDetails"));
            icon.text = "Launcher";
            window.titleContent = icon;
        }
        public static LauncherWindow Instance { get; private set; }

        void OnGUI()
        {
            Instance = this;
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
            // ヘッダー
            DrawProjectHeader(projSettings);
            // タブ表示
            var tabLayout = DrawTabNavi(projSettings, userSettings);

            // コンテンツの表示
            using (var contentScroller = new GUILayout.ScrollViewScope(_contentScrollPosition))
            {
                tabLayout?.OnGUI();

                _contentScrollPosition = contentScroller.scrollPosition;
            }
        }
        void DrawProjectHeader(LauncherProjectSettings projSettings)
        {
            using var headerScan = _headerScoped.Scan();
            if (headerScan.TryGetRect(out Rect headerRect)){
                var tex = projSettings.ProjectInfo.HeaderTexture;
                if (tex != null)
                {
                    GUI.DrawTexture(headerRect, tex, ScaleMode.ScaleAndCrop);
                    EditorGUI.DrawRect(headerRect, new Color(0f, 0f, 0f, 0.6f));
                }
            }
            if (!string.IsNullOrEmpty(projSettings.ProjectInfo.HelpUrl))
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(EditorGUIUtility.IconContent("_Help"), EditorStyles.iconButton))
                {
                    Application.OpenURL(projSettings.ProjectInfo.HelpUrl);
                }
                GUILayout.Space(4);
            }
            else
            {
                GUILayout.Space(16);
            }
            // タイトル
            if (projSettings.ProjectInfo.CustomHeader == null)
            {
                GUILayout.Space(-8);
                using (new EditorGUILayout.HorizontalScope())
                {
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
            }
            else
            {
                projSettings.ProjectInfo.CustomHeader.OnGUI();
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUIUtil.MuteButton();
                GUILayout.FlexibleSpace();
                {
                    using (new EditorGUILayout.VerticalScope())
                    {
                        GUILayout.Space(-2);
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            if (EditorGUIUtil.IconTextButton("d__Popup", "Project "))
                            {
                                SettingsService.OpenProjectSettings(LauncherProjectSettingsProvider.SettingPath);
                            }
                            if (EditorGUIUtil.IconTextButton("d__Popup", "User "))
                            {
                                SettingsService.OpenUserPreferences(LauncherUserSettingsProvider.SettingPath);
                            }
                        }
                    }
                }
            }
            EditorGUIUtil.DrawSeparator();
        }
        ITabLayout DrawTabNavi(LauncherProjectSettings projSettings, LauncherUserSettings userSettings)
        {
            GUIContent TabContent(ITabLayout layout)
                => EditorGUIUtil.GetIconContent16x16(layout.Title, layout.Icon);

            List<ITabLayout> tabs = new(1 + projSettings.ProjectTabs.Length + userSettings.UserTabs.Length)
                {
                    new PortalLayout(),
                };

            tabs.AddRange(projSettings.ProjectTabs.Where(t => t != null));
            tabs.AddRange(userSettings.UserTabs.Where(t => t != null));

            // ツールバー
            int selectedTab = _tabToolBar.OnGUI(tabs.Select(TabContent));

            if (selectedTab < tabs.Count)
            {
                return tabs[selectedTab];
            }
            else
            {
                return null;
            }
        }
        Vector2 _contentScrollPosition;
        TabToolBar _tabToolBar = new();
        RectScope _headerScoped = new();
    }
}
