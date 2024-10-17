using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public struct SimpleHorizontalScroller
    {
        public SimpleHorizontalScrollScope OnGUI(float cotentWidth, float height)
        {
            GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _position = GUILayoutUtility.GetLastRect();
            }
            _position.height = height;

            float deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;
            var scrollView = new SimpleHorizontalScrollScope(_position, _scrollX, cotentWidth, deltaTime, scrollSpeed: 60 * 20);
            _scrollX = scrollView.scrollX;
            return scrollView;
        }
        float _scrollX;
        float _prevTime;
        Rect _position;
    }
}
