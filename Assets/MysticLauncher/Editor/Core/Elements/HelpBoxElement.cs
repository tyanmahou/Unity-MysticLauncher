using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class HelpBoxElement : IElement
    {
        public Icon Icon;
        [TextArea]
        public string Text = string.Empty;
        public void OnGUI()
        {
            var content = EditorGUIUtil.GetIconContent32x32(Text, Icon);
            EditorGUILayout.HelpBox(content, wide: true);
        }
        public override string ToString()
        {
            return Text.Split('\n')[0];
        }
    }
}
