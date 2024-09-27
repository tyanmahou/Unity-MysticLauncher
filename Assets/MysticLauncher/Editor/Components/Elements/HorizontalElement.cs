using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class HorizontalElement : IElement
    {
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            using var horizontal = new EditorGUILayout.HorizontalScope();
            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return "Horizontal";
        }
    }
}
