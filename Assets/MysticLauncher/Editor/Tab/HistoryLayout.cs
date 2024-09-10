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
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_FolderEmpty On Icon"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    CloseToggleAll();
                }
                if (GUILayout.Button(EditorGUIUtility.IconContent("d_FolderOpened Icon"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    OpenToggleAll();
                }

                if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
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
                    if (!_toggle.ContainsKey(openDate))
                    {
                        _toggle[openDate] = true;
                    }
                    if (isChangedSearch && !string.IsNullOrEmpty(_searchString))
                    {
                        _toggle[openDate] = true;
                    }
                    GUIContent folderContent = EditorGUIUtil.FolderTogleContent(_toggle[openDate], openDate);
                    _toggle[openDate] = EditorGUILayout.Foldout(_toggle[openDate], folderContent, true);
                }
                if (_toggle[openDate])
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    DrawEntry(item);
                }
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
            var content = EditorGUIUtility.ObjectContent(entry.Asset, typeof(UnityEngine.Object));
            content.tooltip = entry.OpenedAt;
            if (GUILayout.Button(content, buttonStyle, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                if (Event.current.button == 0)
                {
                    _openEntry = entry;
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
        Dictionary<string, bool> _toggle = new();
    }
}
