using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace Mystic
{
    [Serializable]
    public class FavoriteLayout : ITabLayout
    {
        public string Title => "Favorite";
        public Icon Icon { get; set; } = Icon.CreateUnityIcon("d_Favorite_colored");

        public void OnGUI()
        {
            var userFavorite = UserFavorite.instance;
            if (userFavorite == null)
            {
                return;
            }
            // 検索
            bool isChangedSearch = false;
            GUILayout.Space(5);
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                string prevSearch = _searchString;
                _searchString = _searchField.OnGUI(_searchString);
                isChangedSearch = _searchString != prevSearch;
                if (EditorGUIUtil.IconButton("d_FolderEmpty On Icon", "Close Toggle All"))
                {
                    CloseToggleAll();
                }
                if (EditorGUIUtil.IconButton("d_FolderOpened Icon", "Open Toggle All"))
                {
                    OpenToggleAll();
                }

                if (EditorGUIUtil.IconButton("d_Favorite_colored", "Show Favorite Window"))
                {
                    FavoriteWindow.Show(null);
                }
                if (EditorGUIUtil.IconButton("d__Popup", "Edit"))
                {
                    SettingsService.OpenUserPreferences(UserFavoriteProvider.SettingPath);
                }
            }
            GUILayout.Space(5);
            EditorGUIUtil.DrawSeparator();

            var entries = userFavorite.Entries;
            if (entries.Count() <= 0)
            {
                EditorGUILayout.HelpBox("You can register assets to favorites from the menu by right-click.\nMystic > Favotite", MessageType.Info);
            }
            Dictionary<string, List<FavoriteEntry>> dic = new();
            foreach (var entry in entries)
            {
                if (!dic.TryGetValue(entry.FavoriteGroup, out var list))
                {
                    list = new List<FavoriteEntry>();
                    dic.Add(entry.FavoriteGroup, list);
                }
                list.Add(entry);
            }
            {
                using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition);
                _scrollPosition = scrollView.scrollPosition;

                _groupRange.Clear();
                {
                    _treeView.GroupSelector = f => f.FavoriteGroup;
                    _treeView.DrawElementCallback = DrawEntry;
                    _treeView.ForceOpenToggle = (node) =>
                    {
                        if (isChangedSearch && !string.IsNullOrEmpty(_searchString))
                        {
                            if (node.Entries.Count > 0 || node.Children.Count > 0)
                            {
                                return true;
                            }
                        }
                        return false;
                    };
                    _treeView.DrawGroupDecorater = (node, drawer) =>
                    {
                        using var registRect = ScopedRectRegist(node.Group, string.IsNullOrEmpty(node.Group));
                        drawer(node);
                    };
                    _treeView.OnGUI(entries.Where(SearchFilter));
                }
                if (TryGetDragAndDrop(out var registObjs, out string group))
                {
                    foreach (var obj in registObjs)
                    {
                        userFavorite.Replace(obj, group);
                    }
                    userFavorite.Save();
                }
            }
            {
                Rect dropArea = GUILayoutUtility.GetRect(0.0f, 20.0f, GUILayout.ExpandWidth(true));
                var skin = new GUIStyle(GUI.skin.box);
                skin.richText = true;
                GUI.Label(dropArea, "<b>↑ Drag & Drop Here ↑</b>", skin);
            }
        }
        bool SearchFilter(FavoriteEntry f)
        {
            if (f.Asset != null)
            {
                if (f.Asset.name.IsSearched(_searchString))
                {
                    return true;
                }
            }
            if (f.FavoriteGroup.IsSearched(_searchString))
            {
                return true;
            }
            return false;
        }
        void DrawEntry(FavoriteEntry entry)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.objectField);
            buttonStyle.margin.left = EditorGUI.indentLevel * 15;

            using var horizontal = new EditorGUILayout.HorizontalScope();
            var content = EditorGUIUtility.ObjectContent(entry.Asset, typeof(UnityEngine.Object));
            if (GUILayout.Button(content, buttonStyle, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (Event.current.button == 0)
                {
                    EditorGUIUtility.PingObject(entry.Asset);
                    if (_doubleClick.DoubleClick())
                    {
                        AssetDatabase.OpenAsset(entry.Asset);
                    }
                }
                else
                {
                    ShowContextMenu(entry);
                }
            }
            if (EditorGUIUtil.IconButton("ViewToolZoom On@2x", "Ping Asset"))
            {
                EditorGUIUtility.PingObject(entry.Asset);
            }
            if (EditorGUIUtil.IconButton("d_editicon.sml", "Open Asset"))
            {
                AssetDatabase.OpenAsset(entry.Asset);
            }
        }
        private void ShowContextMenu(FavoriteEntry entry)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Open Asset"), false, () =>
            {
                AssetDatabase.OpenAsset(entry.Asset);
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Edit"), false, () =>
            {
                FavoriteWindow.Show(entry.Asset, entry.FavoriteGroup);
            });
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                var userFavorite = UserFavorite.instance;
                Undo.RecordObject(userFavorite, "Remove FavoriteEntry");
                userFavorite.Unregister(entry);
                userFavorite.Save();
            });
            menu.ShowAsContext();
        }
        public override string ToString()
        {
            return Title;
        }
        void CloseToggleAll() => _treeView.ToggleOff();
        void OpenToggleAll() => _treeView.ToggleOn();
        bool TryGetDragAndDrop(out IReadOnlyList<UnityEngine.Object> objs, out string group)
        {
            objs = null;
            group = null;
            (Rect, string)? target = null;
            foreach (var range in _groupRange)
            {
                if (range.Item1.Contains(Event.current.mousePosition))
                {
                    target = range;
                    if ((DragAndDrop.objectReferences?.Length ?? 0) > 0)
                    {
                        EditorGUI.DrawRect(range.Item1, new Color(0.274f, 0.376f, 0.486f, 0.5f));
                        EditorWindow.mouseOverWindow.Repaint();
                    }
                    break;
                }
            }
            if (target == null)
            {
                return false;
            }
            if (Event.current.type != EventType.DragUpdated && Event.current.type != EventType.DragPerform)
            {
                return false;
            }
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            if (Event.current.type != EventType.DragPerform)
            {
                return false;
            }
            DragAndDrop.AcceptDrag();
            DragAndDrop.activeControlID = 0;
            Event.current.Use();

            objs = DragAndDrop.objectReferences;
            group = target.Value.Item2;
            _treeView.Toggle(group, true);
            return true;
        }
        class ScopedRectRegister : IDisposable
        {
            public ScopedRectRegister(string group, Action<Rect, string> register, bool expend)
            {
                _start = GUILayoutUtility.GetRect(0f, 0f);
                _group = group;
                _register = register;
                _expend = expend;
            }
            public void Dispose()
            {
                var end = GUILayoutUtility.GetRect(0f, 0f, GUILayout.ExpandHeight(_expend));
                _start.yMax = end.yMax;
                _register.Invoke(_start, _group);
            }
            Rect _start;
            string _group;
            Action<Rect, string> _register;
            bool _expend;
        }
        ScopedRectRegister ScopedRectRegist(string group, bool expand = false)
        {
            return new ScopedRectRegister(group, RegistRect, expand);
        }
        void RegistRect(Rect r, string group)
        {
            _groupRange.Add((r, group));
        }

        SearchField _searchField = new();
        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        List<(Rect, string)> _groupRange = new List<(Rect, string)> ();
        private DoubleClickCtrl _doubleClick = new();

        GroupTreeView<FavoriteEntry> _treeView = new();
    }
}
