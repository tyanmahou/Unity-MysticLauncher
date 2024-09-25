using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class HistoryLayout : ITabLayout
    {
        public string Title => "History";
        public Icon Icon => Icon.CreateUnityIcon("UndoHistory");

        public void OnGUI()
        {
            var histories = UserHistories.instance;
            // 検索
            bool isChangedSearch = false;
            GUILayout.Space(5);
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                string prevSearch = _searchString;
                _searchString = EditorGUILayout.TextField(GUIContent.none, _searchString, EditorStyles.toolbarSearchField, GUILayout.MinWidth(0));
                isChangedSearch = _searchString != prevSearch;

                if (EditorGUIUtil.IconButton("d_FolderEmpty On Icon", "Close Toggle All"))
                {
                    CloseToggleAll();
                }
                if (EditorGUIUtil.IconButton("d_FolderOpened Icon", "Open Toggle All"))
                {
                    OpenToggleAll();
                }
                if (EditorGUIUtil.IconButton("TreeEditor.Trash", "Clear histories"))
                {
                    histories.Clear();
                    histories.Save();
                }
            }
            GUILayout.Space(5);
            EditorGUIUtil.DrawSeparator();
            using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition);
            _scrollPosition = scrollView.scrollPosition;

            var entries = histories.EnumerateHistories;
            if (entries.Count() <= 0)
            {
                EditorGUILayout.HelpBox("When you open an asset, it is registered in this history.", MessageType.Info);
                return;
            }
            string openDate = null;
            foreach (var item in entries.Where(SearchFilter))
            {
                var date = item.OpenedAt[0..10];
                if (date != openDate)
                {
                    openDate = date;
                    if (!_toggle.TryGetValue(openDate, out var toggleAnim))
                    {
                        toggleAnim = new(true);
                        _toggle.Add(openDate, toggleAnim);
                    }
                    if (isChangedSearch && !string.IsNullOrEmpty(_searchString))
                    {
                        toggleAnim.IsOn = true;
                    }
                    GUIContent folderContent = EditorGUIUtil.FolderTogleContent(toggleAnim.IsOn, openDate);
                    toggleAnim.IsOn = EditorGUILayout.Foldout(toggleAnim.IsOn, folderContent, true);
                }
                if (EditorGUILayout.BeginFadeGroup(_toggle[openDate].Faded))
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    DrawEntry(item);
                }
                EditorGUILayout.EndFadeGroup();
            }
            if (_removeEntry != null)
            {
                Undo.RecordObject(histories, "Remove HistoryEntry");
                histories.Unregister(_removeEntry);
                histories.Save();
                _removeEntry = null;
            }
            if (_openEntry != null)
            {
                AssetDatabase.OpenAsset(_openEntry.Asset);
                _openEntry = null;
            }
        }
        void DrawEntry(HistoryEntry entry)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.objectField);
            buttonStyle.margin.left = EditorGUI.indentLevel * 15 + 15;
            using var horizontal = new EditorGUILayout.HorizontalScope();
            var content = new GUIContent(EditorGUIUtility.ObjectContent(entry.Asset, typeof(UnityEngine.Object)));
            content.tooltip = entry.OpenedAt;
            if (GUILayout.Button(content, buttonStyle, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (Event.current.button == 0)
                {
                    EditorGUIUtility.PingObject(entry.Asset);
                    if (_doubleClick.DoubleClick())
                    {
                        _openEntry = entry;
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
                _openEntry = entry;
            }
        }
        private void ShowContextMenu(HistoryEntry entry)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Open Asset"), false, () =>
            {
                _openEntry = entry;
            });
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                _removeEntry = entry;
            });
            menu.ShowAsContext();
        }
        void CloseToggleAll() => ToggleAll(false);
        void OpenToggleAll() => ToggleAll(true);
        void ToggleAll(bool isOn)
        {
            foreach (var path in GetAllFolderPath())
            {
                Toggle(path, isOn);
            }
        }
        void Toggle(string group, bool on)
        {
            if (!_toggle.TryGetValue(group, out var toggleAnim))
            {
                _toggle.Add(group, new ToggleAnimBool(on));
            }
            else
            {
                toggleAnim.IsOn = on;
            }
        }
        bool SearchFilter(HistoryEntry entry)
        {
            if (entry.Asset != null)
            {
                if (entry.Asset.name.IndexOf(_searchString, System.StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        IEnumerable<string> GetAllFolderPath()
        {
            return UserHistories.instance.EnumerateHistories.Select(f => f.OpenedAt[0..10]).Distinct();
        }
        public override string ToString()
        {
            return Title;
        }
        HistoryEntry _openEntry;
        HistoryEntry _removeEntry;

        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        Dictionary<string, ToggleAnimBool> _toggle = new();
        private DoubleClickCtrl _doubleClick = new();
    }
}
