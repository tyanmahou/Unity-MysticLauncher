﻿using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class SeperatorElement : IElement
    {
        public Icon Icon;
        [TextArea]
        public string Text = string.Empty;
        public void OnGUI()
        {
            EditorGUIUtil.DrawSeparator();
        }
        public override string ToString()
        {
            return "Seperator";
        }
    }
}
