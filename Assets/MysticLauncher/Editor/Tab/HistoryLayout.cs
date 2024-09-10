using System;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
            foreach (var item in histories.EnumerateHistories)
            {
                DrawEntry(item);
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
        public override string ToString()
        {
            return Title;
        }
        HistoryEntry _openEntry;
        HistoryEntry _removeEntry;
    }
}
