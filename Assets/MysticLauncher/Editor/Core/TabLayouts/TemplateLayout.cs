using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class TemplateLayout : ITabLayout
    {
        [SerializeField] TemplateLayoutAsset _template;

        public string Title => _template == null ? "None Template" : _template.Title.Text;
        public Icon Icon => _template == null ? default: _template.Title.Icon;

        public TemplateLayout() { }
        public TemplateLayout(TemplateLayoutAsset template)
        {
            _template = template;
        }

        public void OnGUI()
        {
            if (_template == null)
            {
                return;
            }
            foreach (var entry in _template.Elements)
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
