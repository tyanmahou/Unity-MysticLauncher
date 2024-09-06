using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class DoubleClickCtrl
    {
        public bool DoubleClick()
        {
            bool isDoubleClick = false;
            double currentTime = EditorApplication.timeSinceStartup;
            if (currentTime - _lastClickTime < doubleClickTime)
            {
                isDoubleClick = true;
            }
            _lastClickTime = currentTime;
            return isDoubleClick;
        }
        private const float doubleClickTime = 0.3f;
        private double _lastClickTime = 0;
    }
}
