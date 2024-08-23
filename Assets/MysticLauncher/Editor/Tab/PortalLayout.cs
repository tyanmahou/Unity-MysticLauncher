using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class PortalLayout : ITabLayout
    {
        public string Title => "Portal";
        public Icon Icon { get; set; } = Icon.CreateUnityIcon("d_Profiler.UIDetails");
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

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
