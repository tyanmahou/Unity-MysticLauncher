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
                Rect buttonRect = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight, _normalStyle);
                if (GUI.Button(buttonRect, itemName, _normalStyle))
                {
                    if (Event.current.button == 0)
                    {
                        _scope.enumValueIndex = _selectedTab;
                        _scope.serializedObject.ApplyModifiedProperties();
                        _path.stringValue = itemName;
                        _path.serializedObject.ApplyModifiedProperties();
                        if (_doubleClick.DoubleClick())
                        {
                            Close();
                        }
                    }
                    else
                    {
                        ShowContextMenu(itemName);
                    }
                }
                if (_path.stringValue == itemName && _scope.enumValueIndex == _selectedTab)
                {
                    EditorGUI.DrawRect(buttonRect, new Color(0, 1, 1, 0.15f));
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

        private DoubleClickCtrl _doubleClick = new();

        private GUIStyle _normalStyle;

        private int _selectedTab = 0;
        private static string[] _itemsProject;
        private static string[] _itemsUser;
    }
}
