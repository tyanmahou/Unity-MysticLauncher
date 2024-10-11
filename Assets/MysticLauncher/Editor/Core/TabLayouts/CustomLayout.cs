using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class CustomLayout : ITabLayout
    {
        [SerializeField] Label _title;
        [NamedArrayElement, SerializeReference, SubclassSelector]

        public IElement[] Elements;

        public string Title => _title.Text;
        public Icon Icon => _title.Icon;

        public CustomLayout() { }
        public CustomLayout(string title, Icon icon)
        {
            _title.Text = title;
            _title.Icon = icon;
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
