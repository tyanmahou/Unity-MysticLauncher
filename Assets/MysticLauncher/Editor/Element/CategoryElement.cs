using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class CategoryElement : IElement
    {
        public Label Label;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] Elements;

        public void OnGUI()
        {
            const float height = 30;
            GUILayout.Space(4);
            {
                var style = new GUIStyle(EditorStyles.helpBox);
                style.margin.left += EditorGUI.indentLevel * 15;
                using var skin = new EditorGUILayout.VerticalScope(style);
                using var horizontal = new EditorGUILayout.HorizontalScope();
                if (Label.Icon.TryGetGUIContent(out var content))
                {
                    Rect iconRect = GUILayoutUtility.GetRect(height, height, GUILayout.Width(height), GUILayout.Height(height));
                    GUI.DrawTexture(iconRect, content.image, ScaleMode.ScaleToFit);
                }
                GUILayout.Label(Label.Text, EditorStyles.boldLabel, GUILayout.Height(height));
            }
            GUILayout.Space(2);
            foreach (var entry in Elements)
            {
                if (entry is CategoryElement)
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    entry?.OnGUI();
                }
                else
                {
                    entry?.OnGUI();
                }
            }
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
