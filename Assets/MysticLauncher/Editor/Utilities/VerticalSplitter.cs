using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class VerticalSplitter
    {
        public VerticalSplitter()
        {
        }
        public VerticalSplitter(float minOffset, float maxOffset)
        {
            _separatorHeightMinOffset = minOffset;
            _separatorHeightMaxOffset = maxOffset;
        }
        public void Begin()
        {
            _positionTmp = GUILayoutUtility.GetRect(0, 0);
            _scrollPos1 = GUILayout.BeginScrollView(_scrollPos1, GUILayout.Height(_separatorHeight));
        }
        public void Split()
        {
            GUILayout.EndScrollView();
            // セパレーター
            DrawSeparator();
            _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.ExpandHeight(true));
        }
        public void End()
        {
            GUILayout.EndScrollView();
            var p = GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _position = _positionTmp;
                _position.height = p.y - _position.y;
            }
        }
        private void DrawSeparator()
        {
            var rect = GUILayoutUtility.GetRect(1f, 1f);
            EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f, 1.333f));
            rect.height += 6;
            rect.y -= 3;

            var controlID = GUIUtility.GetControlID(FocusType.Passive);
            var eventType = Event.current.GetTypeForControl(controlID);


            if (_isDragging || rect.Contains(Event.current.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical, controlID);
            }

            // ドラッグ処理
            switch (eventType)
            {
                case EventType.MouseDown when Event.current.button == 0:
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = controlID;
                        _isDragging = true;
                        Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag when _isDragging:
                    _separatorHeight += Event.current.delta.y;
                    Event.current.Use();
                    break;
                case EventType.MouseUp:
                    _isDragging = false;
                    if (GUIUtility.hotControl == controlID)
                    {
                        GUIUtility.hotControl = 0;
                    }
                    break;
                default:
                    break;
            }
            _separatorHeight = Mathf.Clamp(_separatorHeight, _separatorHeightMinOffset, _position.height - _separatorHeightMaxOffset); // 最小サイズを設定
        }
        float _separatorHeight = 200f;
        float _separatorHeightMinOffset = 50f;
        float _separatorHeightMaxOffset = 50f;
        bool _isDragging = false;
        Vector2 _scrollPos1;
        Vector2 _scrollPos2;

        Rect _positionTmp;
        Rect _position;
    }
}
