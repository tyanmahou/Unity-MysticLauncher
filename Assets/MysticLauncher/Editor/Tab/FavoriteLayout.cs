﻿using System;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor.Rendering;

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
            {
                _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField, GUILayout.MinWidth(0));
            }
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

            var favList = userSettings.Favorite.Entries.Where(SearchFilter).Select(f => f.FavoriteGroup);
            Draw(favList, dic, ref removeEntry, string.Empty);
            if (removeEntry != null)
            {
                userSettings.Favorite.Unregister(removeEntry);
                userSettings.Save();
            }
            _scrollPosition = scrollView.scrollPosition;
        }
        void Draw(IEnumerable<string> favList, Dictionary<string, List<FavoriteEntry>> dic, ref FavoriteEntry removeEntry,  string path)
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
                if (_toggle[nextFullPath])
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    Draw(favList, dic, ref removeEntry, nextFullPath);
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
        IEnumerable<string> GetNextPaths(IEnumerable<string> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list.Where(s => s.StartsWith(path) && s != path);
        }
        string[] CalcNextPathNames(IEnumerable<string> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list
                    .Select(s => s.Substring(path.Length).Split('/')[0])
                    .Distinct()
                    .ToArray();
        }
        GUIContent _folderConetent;
        GUIContent _folderOpenedConetent;

        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        Dictionary<string, bool> _toggle =new();
    }
}
