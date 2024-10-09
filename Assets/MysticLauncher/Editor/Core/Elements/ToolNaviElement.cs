using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    [Serializable]
    public class ToolNaviElement : IElement
    {
        [NamedArrayElement]
        public ActionElement[] Elements;

        public void OnGUI()
        {
            float deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            float minWidth = Elements.Length * 60 + (Elements.Length + 1) * 3;
            float viewWidth = EditorGUIUtility.currentViewWidth;
            float scrollSpeed = 60 * 20;
            bool isScroll = minWidth > viewWidth;

            var scrollStyle = new GUIStyle(GUIStyle.none);
            scrollStyle.alignment = TextAnchor.MiddleCenter;
            scrollStyle.normal.textColor = Color.white;

            var y = GUILayoutUtility.GetRect(0, 0).y;
            var leftScroll = new Rect(0, y, 20, 64);
            var rightScroll = new Rect(viewWidth - 20, y, 20, 64);
            bool useLeftScroll = isScroll && _scrollPosition.x > 0;
            bool useRightScroll = isScroll && _scrollPosition.x < minWidth - viewWidth;

            bool hoverLeft = false;
            bool hoverRight = false;
            using (new EditorGUI.DisabledGroupScope(!useLeftScroll))
            {
                if (GUI.RepeatButton(leftScroll, GUIContent.none, EditorStyles.iconButton))
                {
                    _scrollPosition.x -= scrollSpeed * deltaTime;
                    _scrollPosition.x = Mathf.Max(0, _scrollPosition.x);
                    EditorWindow.focusedWindow.Repaint();
                    hoverLeft = true;
                }
                else if (leftScroll.Contains(Event.current.mousePosition))
                {
                    hoverLeft = true;
                }
            }
            using (new EditorGUI.DisabledGroupScope(!useRightScroll))
            {
                if (GUI.RepeatButton(rightScroll, GUIContent.none, EditorStyles.iconButton))
                {
                    _scrollPosition.x += scrollSpeed * deltaTime;
                    _scrollPosition.x = Mathf.Min(minWidth - viewWidth, _scrollPosition.x);
                    EditorWindow.focusedWindow.Repaint();
                    hoverRight = true;
                }
                else if (rightScroll.Contains(Event.current.mousePosition))
                {
                    hoverRight = true;
                }
            }
            using (var scrollView = new GUILayout.ScrollViewScope(_scrollPosition, GUIStyle.none, GUIStyle.none, GUILayout.MinHeight(64), GUILayout.ExpandHeight(false)))
            {
                using (new GUILayout.HorizontalScope())
                {
                    foreach (var element in Elements)
                    {
                        if (EditorGUIUtil.ButtonSquare(element.LabelOverridedTooltip))
                        {
                            element.Execute();
                        }
                    }
                }
                _scrollPosition = scrollView.scrollPosition;
            }
            Color colorDefault = new Color(0.2196f, 0.2196f, 0.2196f, 1.0f);
            Color colorHover = new Color(0.2196f, 0.2196f, 0.2196f, 0.8f);
            if (useLeftScroll)
            {
                EditorGUI.DrawRect(leftScroll, hoverLeft ? colorHover : colorDefault);
                GUI.Label(leftScroll, EditorGUIUtility.IconContent("d_back"), scrollStyle);
            }
            if (useRightScroll)
            {
                EditorGUI.DrawRect(rightScroll, hoverRight ? colorHover : colorDefault);
                GUI.Label(rightScroll, EditorGUIUtility.IconContent("d_forward"), scrollStyle);
            }
        }
        public override string ToString()
        {
            return "Tool Navi";
        }
        Vector2 _scrollPosition;
        float _prevTime = 0;
    }
}
