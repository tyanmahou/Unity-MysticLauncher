using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// お気に入りツール
    /// </summary>
    public class FavoriteWindow : EditorWindow
    {
        public static void Show(UnityEngine.Object asset, string group = null)
        {
            FavoriteWindow window = GetWindow<FavoriteWindow>("Favorite");
            var icon = new GUIContent(EditorGUIUtility.IconContent("d_Favorite_colored"));
            icon.text = "Favorite";
            window.titleContent = icon;
            window.Init(asset, group);
            window.Show();
        }
        public void Init(UnityEngine.Object asset, string group)
        {
            _asset = asset;
            _group = group ?? string.Empty;
        }
        void OnGUI()
        {
            var userSettings = LauncherUserSettings.instance;

            bool oldEnabled = GUI.enabled;
            GUILayout.Space(5);
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                var content = new GUIContent(EditorGUIUtility.IconContent("DefaultAsset Icon"));
                content.text = "Asset";
                GUILayout.Label(content, GUILayout.MaxWidth(60), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                _asset = EditorGUILayout.ObjectField(_asset, typeof(UnityEngine.Object), allowSceneObjects: false);
            }
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                var content = new GUIContent(EditorGUIUtility.IconContent("d_Folder Icon"));
                content.text = "Group";
                GUILayout.Label(content, GUILayout.MaxWidth(60), GUILayout.Height(EditorGUIUtility.singleLineHeight));
                _group = GUILayout.TextField(_group);
            }
            GUILayout.Space(5);
            EditorGUIUtil.DrawSeparator();
            // スクロールビュー開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            {
                var favList = userSettings.Favorite.Entries.Select(f => f.FavoriteGroup).Append(_group);
                DrawFolder(favList, string.Empty, string.Empty);
            }

            EditorGUILayout.EndScrollView();
            EditorGUIUtil.DrawSeparator();

            bool enabledAsset = _asset != null;
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                var fav = new GUIContent(EditorGUIUtility.IconContent("d_Favorite_colored@2x"));
                fav.text = userSettings.Favorite.IsRegistered(_asset) ? "Update" : "Add";

                GUI.enabled = oldEnabled && enabledAsset;
                if (GUILayout.Button(fav, GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    Undo.RecordObject(userSettings, "Add FavoriteEntry");
                    var entry = userSettings.Favorite.Find(_asset);
                    if (entry != null)
                    {
                        entry.FavoriteGroup = _group;
                    }
                    else
                    {
                        userSettings.Favorite.Register(_asset, _group);
                    }
                    userSettings.Save();
                    Close();
                }

                GUI.enabled = oldEnabled && enabledAsset && userSettings.Favorite.IsRegistered(_asset);
                var unfav = new GUIContent(EditorGUIUtility.IconContent("d_winbtn_mac_close_h"));
                unfav.text = "Remove";

                if (GUILayout.Button(unfav, GUILayout.Width(80), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    Undo.RecordObject(userSettings, "Remove FavoriteEntry");
                    userSettings.Favorite.Unregister(_asset);
                    userSettings.Save();
                    Close();
                }
                GUILayout.FlexibleSpace();
            }
            GUI.enabled = oldEnabled;
        }
        void DrawFolder(IEnumerable<string> favList, string path)
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
                DrawFolder(favList, next, nextFullPath);
            }
        }
        void DrawFolder(IEnumerable<string> favList, string next, string nextFullPath)
        {
            if (!_toggle.ContainsKey(nextFullPath))
            {
                _toggle[nextFullPath] = true;
            }
            var rect = GUILayoutUtility.GetRect(0f, 0f);
            if (_group == nextFullPath)
            {
                rect.xMin = 0;
                rect.width += 4f;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.DrawRect(rect, new Color(0.274f, 0.376f, 0.486f, 1.0f));
            }
            bool isLeaf = CalcNextPathNames(GetNextPaths(favList, nextFullPath), nextFullPath).Count() <= 0;
            GUIContent folderContent = !isLeaf && _toggle[nextFullPath] ? new GUIContent(EditorGUIUtility.IconContent("d_FolderOpened Icon")) : new GUIContent(EditorGUIUtility.IconContent("d_Folder Icon"));
            folderContent.text = next;
            if (!isLeaf)
            {
                bool prevToggle = _toggle[nextFullPath];
                bool nextToggle = EditorGUILayout.Foldout(prevToggle, folderContent, true);
                if (prevToggle != nextToggle)
                {
                    _group = nextFullPath;
                    if (_doubleClick.DoubleClick())
                    {
                        _toggle[nextFullPath] = nextToggle;
                    }
                }
            }
            else
            {
                var buttonRect = GUILayoutUtility.GetRect(0f, 0f);
                buttonRect.xMin = 0;
                buttonRect.width += 4f;
                buttonRect.height = EditorGUIUtility.singleLineHeight;
                if (GUI.Button(buttonRect, GUIContent.none, GUI.skin.label))
                {
                    _group = nextFullPath;
                }
                var style = new GUIStyle(EditorStyles.label);
                style.margin.left = EditorGUI.indentLevel * 15 + 13;
                GUILayout.Label(folderContent, style, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            if (_toggle[nextFullPath])
            {
                using var indent = new EditorGUI.IndentLevelScope();
                DrawFolder(favList, nextFullPath);
            }
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

        UnityEngine.Object _asset;
        string _group = string.Empty;
        private Vector2 _scrollPosition;
        Dictionary<string, bool> _toggle = new();

        private DoubleClickCtrl _doubleClick;
    }
}
