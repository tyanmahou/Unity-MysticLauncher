using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    [Serializable]
    public class ToolNaviElement : IElement
    {
        [NamedArrayElement]
        public ActionElement[] Elements;

        public void OnGUI()
        {
            float deltaTime = (float)EditorApplication.timeSinceStartup - _prevTime;
            _prevTime = (float)EditorApplication.timeSinceStartup;

            float minWidth = Elements.Length * 60 + (Elements.Length + 1) * 3;

            using (var scrollView = new SimpleHorizontalScrollScope(_scrollPosition, minWidth, height: 64, deltaTime, scrollSpeed: 60 * 20))
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
                _scrollPosition = scrollView.scrollPosition;
            }
        }
        public override string ToString()
        {
            return "Tool Navi";
        }
        Vector2 _scrollPosition;
        float _prevTime = 0;
    }
}
