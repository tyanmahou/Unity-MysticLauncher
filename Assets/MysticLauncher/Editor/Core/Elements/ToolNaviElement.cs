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
            float minWidth = Elements.Length * 60 + (Elements.Length + 1) * 3;

            using (var scrollView = _scroller.OnGUI(minWidth, height: 64))
            {
                using (new GUILayout.HorizontalScope())
                {
                    foreach (var element in Elements)
                    {
                        if (EditorGUIUtil.ButtonSquare(element.LabelOverridedTooltip))
                        {
                            element.Execute();
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            return "Tool Navi";
        }
        SimpleHorizontalScroller _scroller;
    }
}
