using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class TabToolBar
    {
        public ITabLayout OnGUI(IReadOnlyList<ITabLayout> tabs)
        {
            _deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            //タブの表示
            _selectedTab = EditorGUIUtil.ScrollToolBar(
               _selectedTab,
               ref _scrollTabX,
               _deltaTime,
               tabs.Select(TabContent),
               i => _selectedTab = i
               );
            if (_selectedTab < tabs.Count)
            {
                return tabs[_selectedTab];
            }
            else
            {
                return null;
            }
        }
        GUIContent TabContent(ITabLayout layout)
            => EditorGUIUtil.GetIconContent16x16(layout.Title, layout.Icon);

        int _selectedTab;
        float _scrollTabX = 0;
        float _deltaTime = 0;
        float _prevTime = 0;
    }
}
