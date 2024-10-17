using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public struct TabToolBar
    {
        public int OnGUI(int selected, IEnumerable<GUIContent> contents, Action<int> onContext = null)
        {
            _deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _position = GUILayoutUtility.GetLastRect();
            }
            //タブの表示
            selected = EditorGUIUtil.ScrollToolBar(
                _position,
               selected,
               ref _scrollTabX,
               _deltaTime,
               contents,
               onContext
               );
            return selected;
        }
        float _scrollTabX;
        float _deltaTime;
        float _prevTime;
        Rect _position;
    }
}
