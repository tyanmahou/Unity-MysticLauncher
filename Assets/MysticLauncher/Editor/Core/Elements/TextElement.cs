using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class TextElement : IElement
    {
        [TextArea]
        public string Text = string.Empty;

        public void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.wordWrap = true;
            labelStyle.richText = true;
            var height = labelStyle.CalcHeight(new GUIContent(Text), EditorGUIUtility.currentViewWidth);
            EditorGUILayout.SelectableLabel(Text, labelStyle, GUILayout.Height(height));
        }
        public override string ToString()
        {
            return Text.Split('\n')[0];
        }
    }
}
