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
            using var scrollView = new GUILayout.ScrollViewScope(_scrollPosition, GUILayout.MinHeight(64), GUILayout.ExpandHeight(false));
            _scrollPosition = scrollView.scrollPosition;
            using var h = new GUILayout.HorizontalScope();
            foreach (var element in Elements) 
            {
                if (EditorGUIUtil.ButtonSquare(element.Label))
                {
                    element.Execute();
                }
            }
        }
        public override string ToString()
        {
            return "Tool Navi";
        }
        Vector2 _scrollPosition;
    }
}
