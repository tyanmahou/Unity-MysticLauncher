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
            var userSettings = LauncherUserSettings.instance;
            if (userSettings == null)
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
            _folderConetent ??= new GUIContent(EditorGUIUtility.IconContent("d_Folder Icon"));
            _folderOpenedConetent ??= new GUIContent(EditorGUIUtility.IconContent("d_FolderOpened Icon"));

            Dictionary<string, List<FavoriteEntry>> dic = new();
            foreach (var entry in userSettings.Favorite.Entries)
            {
                if (!dic.TryGetValue(entry.FavoriteGroup, out var list))
                {
                    list = new List<FavoriteEntry>();
                    dic.Add(entry.FavoriteGroup, list);
                }
                list.Add(entry);
            }

            using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition);
            FavoriteEntry removeEntry = null;

            var favList = userSettings.Favorite.Entries.Where(SearchFilter);
            Draw(favList, dic, ref removeEntry, string.Empty, isChangedSearch);
            if (removeEntry != null)
            {
                userSettings.Favorite.Unregister(removeEntry);
                userSettings.Save();
            }
            _scrollPosition = scrollView.scrollPosition;
        }
        void Draw(IEnumerable<FavoriteEntry> favList, Dictionary<string, List<FavoriteEntry>> dic, ref FavoriteEntry removeEntry,  string path, bool isChangedSearch)
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
                GUIContent folderContent = _toggle[nextFullPath] ? _folderOpenedConetent : _folderConetent;
                folderContent.text = next;
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
                    Draw(favList, dic, ref removeEntry, nextFullPath, isChangedSearch);
                }
            }
            if (dic.TryGetValue(path, out var list))
            {
                foreach (var entry in list.Where(SearchFilter))
                {
                    if (!DrawEntry(entry))
                    {
                        removeEntry = entry;
                    }
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
        bool DrawEntry(in FavoriteEntry entry)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.objectField);
            buttonStyle.margin.left = EditorGUI.indentLevel * 15 + 15;

            using var horizontal = new EditorGUILayout.HorizontalScope();
            var content = EditorGUIUtility.ObjectContent(entry.Asset, typeof(UnityEngine.Object));
            if (GUILayout.Button(content, buttonStyle, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (Event.current.button == 0)
                {
                    AssetDatabase.OpenAsset(entry.Asset);
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
            //if (GUILayout.Button(EditorGUIUtility.IconContent("d_editicon.sml"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            //{
            //    FavoriteWindow.Show(entry.Asset);
            //}
            //if (GUILayout.Button(EditorGUIUtility.IconContent("d_winbtn_mac_close_h"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            //{
            //    return false;
            //}
            return true;
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
                var userSettings = LauncherUserSettings.instance;
                Undo.RecordObject(userSettings, "Remove FavoriteEntry");
                userSettings.Favorite.Unregister(entry);
                userSettings.Save();
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
            return GetChildFolderPath(LauncherUserSettings.instance.Favorite.Entries, string.Empty);
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
        static GUIContent _folderConetent;
        static GUIContent _folderOpenedConetent;

        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        Dictionary<string, bool> _toggle =new();
    }
}
