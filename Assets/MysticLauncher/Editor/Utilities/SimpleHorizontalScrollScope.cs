using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class SimpleHorizontalScrollScope : IDisposable
    {
        public SimpleHorizontalScrollScope(in Rect position, float scrollX, float contentWidth, float deltaTime, float scrollSpeed = 1200) 
        {
            _position = position;
            this.scrollX = scrollX;
            float viewWidth = position.width;
            bool isScroll = contentWidth > viewWidth;

            if (_scrollStyle is null)
            {
                _scrollStyle = new GUIStyle(GUIStyle.none);
                _scrollStyle.alignment = TextAnchor.MiddleCenter;
                _scrollStyle.normal.textColor = Color.white;
            }
            _leftScroll = new Rect(_position.x, _position.y, 20, _position.height);
            _rightScroll = new Rect(_position.x + viewWidth - 20, _position.y, 20, _position.height);
            _useLeftScroll = isScroll && this.scrollX > 0;
            _useRightScroll = isScroll && this.scrollX < contentWidth - viewWidth;

            _hoverLeft = false;
            _hoverRight = false;
            using (new EditorGUI.DisabledGroupScope(!_useLeftScroll))
            {
                if (GUI.RepeatButton(_leftScroll, GUIContent.none, EditorStyles.iconButton))
                {
                    this.scrollX -= scrollSpeed * deltaTime;
                    this.scrollX = Mathf.Max(0, this.scrollX);
                    EditorWindow.focusedWindow.Repaint();
                    _hoverLeft = true;
                }
                else if (_leftScroll.Contains(Event.current.mousePosition))
                {
                    _hoverLeft = true;
                }
            }
            using (new EditorGUI.DisabledGroupScope(!_useRightScroll))
            {
                if (GUI.RepeatButton(_rightScroll, GUIContent.none, EditorStyles.iconButton))
                {
                    this.scrollX += scrollSpeed * deltaTime;
                    this.scrollX = Mathf.Min(contentWidth - viewWidth, this.scrollX);
                    EditorWindow.focusedWindow.Repaint();
                    _hoverRight = true;
                }
                else if (_rightScroll.Contains(Event.current.mousePosition))
                {
                    _hoverRight = true;
                }
            }
            var scrollPosition = GUILayout.BeginScrollView(this.scrollX * Vector2.right, GUIStyle.none, GUIStyle.none, GUILayout.MinHeight(_position.height), GUILayout.ExpandHeight(false));
            this.scrollX = scrollPosition.x;
        }

        public void Dispose() 
        {
            GUILayout.EndScrollView();

            Color colorDefault = new Color(0.2196f, 0.2196f, 0.2196f, 1.0f);
            Color colorHover = new Color(0.2196f, 0.2196f, 0.2196f, 0.8f);
            if (_useLeftScroll)
            {
                EditorGUI.DrawRect(_leftScroll, _hoverLeft ? colorHover : colorDefault);
                GUI.Label(_leftScroll, EditorGUIUtility.IconContent("d_back"), _scrollStyle);
            }
            if (_useRightScroll)
            {
                EditorGUI.DrawRect(_rightScroll, _hoverRight ? colorHover : colorDefault);
                GUI.Label(_rightScroll, EditorGUIUtility.IconContent("d_forward"), _scrollStyle);
            }
        }
        Rect _position;
        public float scrollX;
        static GUIStyle _scrollStyle;
        Rect _leftScroll;
        Rect _rightScroll;
        bool _hoverLeft = false;
        bool _hoverRight = false;
        bool _useLeftScroll = false;
        bool _useRightScroll = false;
    }
}
