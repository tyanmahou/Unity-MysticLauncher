using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class VerticalSplitter
    {
        public VerticalSplitter():
            this(50, 50)
        {
        }
        public VerticalSplitter(float minOffset, float maxOffset)
        {
            _separatorHeight = 200;
            _separatorMin = h => minOffset;
            _separatorMax = h => h - maxOffset;
        }
        public VerticalSplitter(
            Func<float, float> separatorInit,
            Func<float, float> separatorMin,
            Func<float, float> separatorMax            
            )
        {
            _separatorInit = separatorInit;
            _separatorMin = separatorMin;
            _separatorMax = separatorMax;
        }
        public class ScopedTopView : IDisposable
        {
            internal ScopedTopView(VerticalSplitter splitter)
            {
                splitter.BeginTopView();
            }
            public void Dispose() 
            {
                GUILayout.EndScrollView();
            }
        }
        public ScopedTopView SplitTop()
        {
            return new(this);
        }
        public class ScopedBottomView : IDisposable
        {
            internal ScopedBottomView(VerticalSplitter splitter)
            {
                _splitter = splitter;
                _splitter.BeginBottomView();
            }
            public void Dispose()
            {
                _splitter.EndBottomView();
            }
            VerticalSplitter _splitter;
        }
        public ScopedBottomView SplitBottom()
        {
            return new(this);
        }

        private void BeginTopView()
        {
            _positionTmp = GUILayoutUtility.GetRect(0, 0);
            _scrollPos1 = GUILayout.BeginScrollView(_scrollPos1, GUILayout.Height(_separatorHeight));
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
            _separatorHeight = Mathf.Clamp(
                _separatorHeight,
                Mathf.Max(_separatorMin?.Invoke(_position.height) ?? 1, 1),
                Mathf.Min(_separatorMax?.Invoke(_position.height) ?? _position.height -1, _position.height-1)
                );
        }

        private void BeginBottomView()
        {
            // セパレーター
            DrawSeparator();
            _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, GUILayout.ExpandHeight(true));
        }
        private void EndBottomView()
        {
            GUILayout.EndScrollView();
            var p = GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _position = _positionTmp;
                _position.height = p.y - _position.y;
                if (!_init)
                {
                    _separatorHeight = _separatorInit?.Invoke(_position.height) ?? _separatorHeight;
                }
                _init = true;
            }
        }

        float _separatorHeight;
        Func<float, float> _separatorMin;
        Func<float, float> _separatorMax;
        Func<float, float> _separatorInit;
        bool _isDragging = false;
        Vector2 _scrollPos1;
        Vector2 _scrollPos2;

        Rect _positionTmp;
        Rect _position;
        bool _init = false;
    }
}
