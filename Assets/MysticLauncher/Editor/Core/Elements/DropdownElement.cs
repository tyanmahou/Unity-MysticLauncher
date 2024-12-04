using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class DropdownElement : IElement
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
            return "Dropbox";
        }
        Tab DrawTabNavi()
        {
            int selectedTab = _dropDown.OnGUI(Tabs.Select(t => t.Title.GetGUIContent()));
            if (selectedTab < Tabs.Length)
            {
                return Tabs[selectedTab];
            }
            else
            {
                return null;
            }
        }
        TabDropdown _dropDown = new();
    }
}
