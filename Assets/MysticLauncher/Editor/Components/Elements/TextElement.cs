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
            EditorGUILayout.LabelField(Text, labelStyle);
        }
        public override string ToString()
        {
            return Text.Split('\n')[0];
        }
    }
}
