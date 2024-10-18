using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class VerticalElement : IElement
    {
        public float Width = 0;
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            using var vertical = new EditorGUILayout.VerticalScope(GUILayout.Width(Width));
            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return "Vertical";
        }
    }
}
