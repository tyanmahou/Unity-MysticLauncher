using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

namespace Mystic
{
    /// <summary>
    /// SettingServiceピッカー
    /// </summary>
    public class SettingServicePickerWindow : EditorWindow
    {
        private const float doubleClickTime = 0.3f;
        static SettingServicePickerWindow()
        {
            _itemsUser = FindMenuItems(SettingsScope.User);
            _itemsProject = FindMenuItems(SettingsScope.Project);
        }
        public static void Show(SerializedProperty property)
        {
            SettingServicePickerWindow window = GetWindow<SettingServicePickerWindow>("SettingService Picker");
            window.Init(property);
            window.Show();
        }
        public void Init(SerializedProperty property)
        {
            _property = property;
            _scope = property.FindPropertyRelative("Scope");
            _path = property.FindPropertyRelative("SettingPath");

            // カスタムスタイルの定義
            _selectedStyle = new GUIStyle(EditorStyles.objectField);
            _selectedTex = EditorGUIUtil.MakeTex(2, 2, new Color(0.274f, 0.376f, 0.486f, 1.0f));
            _selectedStyle.normal.background = _selectedTex;

            _normalStyle = new GUIStyle(EditorStyles.objectField);
        }

        void OnGUI()
        {
            _selectedTab = GUILayout.Toolbar(
                _selectedTab,
                new string[] { "User", "Project" },
                EditorStyles.toolbarButton,
                GUI.ToolbarButtonSize.FitToContents
                );
            // 検索バーの描画
            {
                _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField);
            }
            EditorGUIUtil.DrawSeparator();
            // スクロールビュー開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (string itemName in GetFilteredItems(_searchString)) 
            {
                var skin = _normalStyle;
                if (_path.stringValue == itemName && _scope.enumValueIndex == _selectedTab)
                {
                    skin = _selectedStyle;
                }
                if (GUILayout.Button(itemName, skin))
                {
                    if (Event.current.button == 0)
                    {
                        double currentTime = EditorApplication.timeSinceStartup;
                        _scope.enumValueIndex = _selectedTab;
                        _scope.serializedObject.ApplyModifiedProperties();
                        _path.stringValue = itemName;
                        _path.serializedObject.ApplyModifiedProperties();
                        if (currentTime - _lastClickTime < doubleClickTime)
                        {
                            Close();
                        }
                        _lastClickTime = currentTime;
                    }
                    else
                    {
                        ShowContextMenu(itemName);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }
        private void OnLostFocus()
        {
            Close();
        }
        private void OnDestroy()
        {
            DestroyImmediate(_selectedTex);
        }
        private void ShowContextMenu(string itemName)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy MenuItem Name"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = itemName;
            });

            menu.ShowAsContext();
        }
        private string[] GetFilteredItems(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return Items;
            }
            else
            {
                return Items.Where(c => c.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            }
        }
        private string[] Items => _selectedTab == 0 ? _itemsUser : _itemsProject;
        static string[] FindMenuItems(SettingsScope scope)
        {
            return FetchSettingsProviders(scope).Select(s => s.settingsPath).OrderBy(s => s).ToArray();
        }
        static SettingsProvider[] FetchSettingsProviders(SettingsScope scope)
        {
            Type settingsService = typeof(SettingsService);
            MethodInfo fetchMethod = settingsService.GetMethod(
                "FetchSettingsProviders",
                BindingFlags.NonPublic | BindingFlags.Static,
                null,
                new Type[] { typeof(SettingsScope) },
                null
            );

            if (fetchMethod == null)
            {
                throw new InvalidOperationException("FetchSettingsProviders method not found.");
            }
            return (SettingsProvider[])fetchMethod.Invoke(null, new object[] { scope });
        }
        private SerializedProperty _property;
        private SerializedProperty _scope;
        private SerializedProperty _path;

        private string _searchString = "";
        private Vector2 _scrollPosition;

        private double _lastClickTime = 0;

        private GUIStyle _normalStyle;
        private GUIStyle _selectedStyle;
        private Texture2D _selectedTex;

        private int _selectedTab = 0;
        private static string[] _itemsProject;
        private static string[] _itemsUser;
    }
}
