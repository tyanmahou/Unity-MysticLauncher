using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class TemplateElement : IElement
    {
        [SerializeField] TemplateElementAsset _template;

        public TemplateElement() { }
        public TemplateElement(TemplateElementAsset template)
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
            return _template == null ? "None Template" : _template.name;
        }
    }
}
