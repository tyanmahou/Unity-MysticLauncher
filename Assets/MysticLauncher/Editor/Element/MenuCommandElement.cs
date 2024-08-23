using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class MenuCommandElement : IElement
    {
        public Label Label;
        public string Command;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
            {
                EditorApplication.ExecuteMenuItem(Command);
            }
        }
    }
}
