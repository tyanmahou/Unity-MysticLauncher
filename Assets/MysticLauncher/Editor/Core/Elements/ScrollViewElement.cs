using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class ScrollViewElement : IElement
    {
        public float Height = 0;
        public bool ExpandHeight = false;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            using var scroller = new EditorGUILayout.ScrollViewScope(_scrollPosition, GUILayout.Height(Height), GUILayout.ExpandHeight(ExpandHeight));
            _scrollPosition = scroller.scrollPosition;

            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return "Scroll";
        }
        Vector2 _scrollPosition;
    }
}
