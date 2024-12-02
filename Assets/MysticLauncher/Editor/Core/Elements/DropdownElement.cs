using System;
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
            if (_selectedTab >= Tabs.Length)
            {
                return null;
            }
            bool dropdown = EditorGUILayout.DropdownButton(Tabs[_selectedTab].Title.GetGUIContent(), FocusType.Passive);
            if (Event.current.type == EventType.Repaint)
            {
                _position = GUILayoutUtility.GetLastRect();
                _position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            if (dropdown)
            {
                DropdownPopup.Show(_position, Tabs, i => _selectedTab = i);
            }
            if (_selectedTab < Tabs.Length)
            {
                return Tabs[_selectedTab];
            }
            else
            {
                return null;
            }
        }
        int _selectedTab = 0;
        Rect _position;
        public class DropdownPopup : PopupWindowContent
        {
            public static void Show(Rect position, Tab[] tab, Action<int> selected)
            {
                Rect pos = position;
                pos.height = 0;
                pos.width = 0;

                position.height = (4 + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * tab.Length + 10 - EditorGUIUtility.standardVerticalSpacing;
                PopupWindow.Show(pos, new DropdownPopup(position, tab, selected));
            }
            DropdownPopup(Rect position, Tab[] tab, Action<int> selected)
            {
                _position = GUIUtility.GUIToScreenRect(position);
                _tab = tab;
                _selected = selected;
            }

            public override Vector2 GetWindowSize()
            {
                return _position.size;
            }
            public override void OnGUI(Rect rect)
            {
                rect.x += 5;
                rect.width -= 10;
                rect.y += 5;
                rect.height = EditorGUIUtility.singleLineHeight + 4;
                for(int i = 0; i < _tab.Length; ++i)
                {
                    var content = _tab[i].Title.GetGUIContent();
                    if (GUI.Button(rect, content))
                    {
                        _selected?.Invoke(i);
                        editorWindow.Close();
                    }
                    rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            Rect _position;
            Tab[] _tab;
            Action<int> _selected;
        }
    }
}
