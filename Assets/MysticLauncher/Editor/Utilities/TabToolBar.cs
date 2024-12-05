using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class TabToolBar
    {
        public int OnGUI(IEnumerable<GUIContent> contents)
        {
            _deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            GUILayoutUtility.GetRect(0, 0);
            if (Event.current.type == EventType.Repaint)
            {
                _position = GUILayoutUtility.GetLastRect();
            }
            //タブの表示
            _selected = EditorGUIUtil.ScrollToolBar(
                _position,
               _selected,
               ref _scrollTabX,
               _deltaTime,
               contents,
               i => _selected = i
               );
            return _selected;
        }
        int _selected = 0;
        float _scrollTabX;
        float _deltaTime;
        float _prevTime;
        Rect _position;
    }
}
