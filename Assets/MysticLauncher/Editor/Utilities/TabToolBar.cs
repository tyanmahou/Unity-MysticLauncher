using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class TabToolBar
    {
        public int OnGUI(int selected, IEnumerable<GUIContent> contents, Action<int> onContext = null)
        {
            _deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            //タブの表示
            selected = EditorGUIUtil.ScrollToolBar(
               selected,
               ref _scrollTabX,
               _deltaTime,
               contents,
               onContext
               );
            return selected;
        }
        float _scrollTabX = 0;
        float _deltaTime = 0;
        float _prevTime = 0;
    }
}
