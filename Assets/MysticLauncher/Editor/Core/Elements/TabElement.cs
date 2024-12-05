using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class TabElement : IElement
    {
        [Serializable]
        public class Tab
        {
            public Label Title;
            [NamedArrayElement, SerializeReference, SubclassSelector]
            public IElement[] Elements = new IElement[0];
        }
        [NamedArrayElement]
        public Tab[] Tabs = new Tab[0];
        public void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                var currentTab = DrawTabNavi();
                if (currentTab != null)
                {
                    foreach (var element in currentTab.Elements)
                    {
                        element?.OnGUI();
                    }
                }
            }
        }
        public override string ToString()
        {
            return "Tab";
        }
        Tab DrawTabNavi()
        {
            int selectedTab = _tabToolBar.OnGUI(Tabs.Select(t => EditorGUIUtil.GetIconContent16x16(t.Title)));

            if (selectedTab < Tabs.Length)
            {
                return Tabs[selectedTab];
            }
            else
            {
                return null;
            }
        }
        TabToolBar _tabToolBar = new();
    }
}
