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
                _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField, GUILayout.MinWidth(0));
                isChangedSearch = _searchString != prevSearch;
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_FolderEmpty On Icon"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    CloseToggleAll();
                }
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_FolderOpened Icon"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    OpenToggleAll();
                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("d_Favorite_colored"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    FavoriteWindow.Show(null);
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
                    using var registRect = ScopedRectRegist(string.Empty, true);
                    var favList = entries.Where(SearchFilter);
                    Draw(favList, dic, string.Empty, isChangedSearch);
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
                GUI.Box(dropArea, "<b>↑ Drag & Drop Here ↑</b>", skin);
            }
        }
        void Draw(IEnumerable<FavoriteEntry> favList, Dictionary<string, List<FavoriteEntry>> dic, string path, bool isChangedSearch)
        {
            favList = GetNextPaths(favList, path);
            foreach (string next in CalcNextPathNames(favList, path))
            {
                string nextFullPath = path;
                if (string.IsNullOrEmpty(path))
                {
                    nextFullPath = next;
                }
                else
                {
                    nextFullPath += "/" + next;
                }
                if (!_toggle.ContainsKey(nextFullPath)) {
                    _toggle[nextFullPath] = false;
                }

                {
                    using var registRect = ScopedRectRegist(nextFullPath);
                    GUIContent folderContent = EditorGUIUtil.FolderTogleContent(_toggle[nextFullPath], next);
                    {
                        _toggle[nextFullPath] = EditorGUILayout.Foldout(_toggle[nextFullPath], folderContent, true);
                    }
                    if (isChangedSearch && !string.IsNullOrEmpty(_searchString))
                    {
                        if (favList.Where(SearchFilterAssetName).Count() > 0)
                        {
                            _toggle[nextFullPath] = true;
                        }
                    }
                    if (_toggle[nextFullPath])
                    {
                        using var indent = new EditorGUI.IndentLevelScope();
                        Draw(favList, dic, nextFullPath, isChangedSearch);
                    }
                }
            }
            if (dic.TryGetValue(path, out var list))
            {
                foreach (var entry in list.Where(SearchFilter))
                {
                    DrawEntry(entry);
                }
            }
        }
        bool SearchFilter(FavoriteEntry f)
        {
            if (f.Asset != null)
            {
                if (f.Asset.name.IndexOf(_searchString, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            if (f.FavoriteGroup.IndexOf(_searchString, System.StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
        bool SearchFilterAssetName(FavoriteEntry f)
        {
            if (f.Asset != null)
            {
                if (f.Asset.name.IndexOf(_searchString, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        void DrawEntry(in FavoriteEntry entry)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.objectField);
            buttonStyle.margin.left = EditorGUI.indentLevel * 15 + 15;

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
            if (GUILayout.Button(EditorGUIUtility.IconContent("ViewToolZoom On@2x"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                EditorGUIUtility.PingObject(entry.Asset);
            }
            if (GUILayout.Button(EditorGUIUtility.IconContent("d_editicon.sml"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
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
        IEnumerable<FavoriteEntry> GetNextPaths(IEnumerable<FavoriteEntry> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list.Where(f => f.FavoriteGroup.StartsWith(path) && f.FavoriteGroup != path);
        }
        IEnumerable<string> CalcNextPathNames(IEnumerable<FavoriteEntry> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list
                .Select(f => f.FavoriteGroup)
                .Select(s => s.Substring(path.Length).Split('/')[0])
            .Distinct()
                ;
        }
        void CloseToggleAll()
        {
            foreach (var path in GetAllFolderPath())
            {
                _toggle[path] = false;
            }
        }
        void OpenToggleAll()
        {
            foreach (var path in GetAllFolderPath())
            {
                _toggle[path] = true;
            }
        }
        IEnumerable<string> GetAllFolderPath()
        {
            return GetChildFolderPath(UserFavorite.instance.Entries, string.Empty);
        }
        IEnumerable<string> GetChildFolderPath(IEnumerable<FavoriteEntry> favList, string path)
        {
            favList = GetNextPaths(favList, path);
            foreach (string next in CalcNextPathNames(favList, path))
            {
                string nextFullPath = path;
                if (string.IsNullOrEmpty(path))
                {
                    nextFullPath = next;
                }
                else
                {
                    nextFullPath += "/" + next;
                }
                yield return nextFullPath;

                foreach (var p in GetChildFolderPath(favList, nextFullPath))
                {
                    yield return p;
                }
            }
        }
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
            _toggle[group] = true;
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

        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        Dictionary<string, bool> _toggle =new();
        List<(Rect, string)> _groupRange = new List<(Rect, string)> ();
        private DoubleClickCtrl _doubleClick = new();
    }
}
