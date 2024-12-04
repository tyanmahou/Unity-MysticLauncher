using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public struct TabDropdown
    {
        public void OnGUI(int selected, IReadOnlyList<GUIContent> contents, Action<int> selectedCallback = null)
        {
            bool dropdown = EditorGUILayout.DropdownButton(contents[selected], FocusType.Passive);
            if (Event.current.type == EventType.Repaint)
            {
                _position = GUILayoutUtility.GetLastRect();
                _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            if (dropdown)
            {
                DropdownPopup.Show(_position, contents, selectedCallback);
            }
        }
        Rect _position;

        class DropdownPopup : PopupWindowContent
        {
            public static void Show(Rect position, IReadOnlyList<GUIContent> contents, Action<int> selected)
            {
                Rect pos = position;
                pos.height = 0;
                pos.width = 0;

                position.height = (4 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * contents.Count + 10 - EditorGUIUtility.standardVerticalSpacing;
                PopupWindow.Show(pos, new DropdownPopup(position, contents, selected));
            }
            DropdownPopup(Rect position, IReadOnlyList<GUIContent> contents, Action<int> selected)
            {
                _position = GUIUtility.GUIToScreenRect(position);
                _contents = contents;
                _selected = selected;
            }

            public override Vector2 GetWindowSize()
            {
                return _position.size;
            }
            public override void OnGUI(Rect rect)
            {
                rect.x += 5;
                rect.width -= 10;
                rect.y += 5;
                rect.height = EditorGUIUtility.singleLineHeight + 4;
                for (int i = 0; i < _contents.Count; ++i)
                {
                    if (GUI.Button(rect, _contents[i]))
                    {
                        _selected?.Invoke(i);
                        editorWindow.Close();
                    }
                    rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            Rect _position;
            IReadOnlyList<GUIContent> _contents;
            Action<int> _selected;
        }
    }
}
