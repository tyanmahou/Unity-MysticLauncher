using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
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
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
