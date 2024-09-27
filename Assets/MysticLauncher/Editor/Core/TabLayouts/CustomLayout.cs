using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class CustomLayout : ITabLayout
    {
        [SerializeField] string _title;
        [SerializeField] Icon _icon;
        [NamedArrayElement, SerializeReference, SubclassSelector]

        public IElement[] Elements;

        public string Title => _title;
        public Icon Icon => _icon;

        public CustomLayout() { }
        public CustomLayout(string title, Icon icon)
        {
            _title = title;
            _icon = icon;
        }

        public void OnGUI()
        {
            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
