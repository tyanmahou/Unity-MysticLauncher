using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class BoxElement : IElement
    {
        public Label Label;
        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            using var vertical = new EditorGUILayout.VerticalScope(GUI.skin.box);
            if (!string.IsNullOrEmpty(Label.Text) || Label.Icon.IsValid)
            {
                var content = EditorGUIUtil.GetIconContent16x16(Label);
                EditorGUILayout.LabelField(content, EditorStyles.boldLabel);
            }
            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return "Box";
        }
    }
}
