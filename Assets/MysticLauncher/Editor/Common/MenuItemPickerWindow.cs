using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Mystic
{
    /// <summary>
    /// メニューアイテムピッカー
    /// </summary>
    public class MenuItemPickerWindow : EditorWindow
    {
        static MenuItemPickerWindow()
        {
            _items ??= FindMenuItems();
        }
        public static void Show(SerializedProperty property)
        {
            MenuItemPickerWindow window = GetWindow<MenuItemPickerWindow>("Menu Item Picker");
            window.Init(property);
            window.Show();
        }
        public void Init(SerializedProperty property)
        {
            _property = property;
            // カスタムスタイルの定義
            _selectedStyle = new GUIStyle(EditorStyles.objectField);
            _selectedTex = EditorGUIUtil.MakeTex(2, 2, new Color(0.274f, 0.376f, 0.486f, 1.0f));
            _selectedStyle.normal.background = _selectedTex;

            _normalStyle = new GUIStyle(EditorStyles.objectField);
        }

        void OnGUI()
        {
            // 検索バーの描画
            {
                _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField);
            }
            EditorGUIUtil.DrawSeparator();

            // スクロールビュー開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            foreach (string itemName in GetFilteredItems(_searchString).OrderBy(s => s)) 
            {
                var skin = _normalStyle;
                if (_property.stringValue == itemName)
                {
                    skin = _selectedStyle;
                }
                if (GUILayout.Button(itemName, skin))
                {
                    if (Event.current.button == 0)
                    {
                        _property.stringValue = itemName;
                        _property.serializedObject.ApplyModifiedProperties();
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
                return _items;
            }
            else
            {
                return _items.Where(c => c.IndexOf(search, System.StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            }
        }
        static string[] FindMenuItems()
        {
            List<string> items = new List<string>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // 全てのstaticメソッドを取得
            foreach (MethodInfo method in assemblies
                .SelectMany(a => a.GetTypes())
                .SelectMany(t => t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
                )
            {
                // MenuItemアトリビュートが付いているか確認
                object[] attributes = method.GetCustomAttributes(typeof(MenuItem), false);
                if (attributes.Length > 0)
                {
                    foreach (MenuItem menuItem in attributes)
                    {
                        items.Add(menuItem.menuItem);
                    }
                }
            }
            return items.Distinct().OrderBy(s => s).ToArray();
        }
        private SerializedProperty _property;
        private string _searchString = "";
        private Vector2 _scrollPosition;

        private DoubleClickCtrl _doubleClick = new();

        private GUIStyle _normalStyle;
        private GUIStyle _selectedStyle;
        private Texture2D _selectedTex;

        private static string[] _items;
    }
}
