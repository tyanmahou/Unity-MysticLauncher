using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class ToolNaviElement : IElement
    {
        [NamedArrayElement]
        public ActionElement[] Elements;

        public void OnGUI()
        {
            using var h = new GUILayout.HorizontalScope();

            foreach (var element in Elements) 
            {
                if (EditorGUIUtil.ButtonSquare(element.Label, element.Tooltip))
                {
                    element.Execute();
                }
            }
        }
        public override string ToString()
        {
            return "Tool Navi";
        }
    }
}
