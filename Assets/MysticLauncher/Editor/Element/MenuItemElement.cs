using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class MenuItemElement : IElement
    {
        public Label Label;

        [MenuItemPicker]
        public string ItemName;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
            {
                EditorApplication.ExecuteMenuItem(ItemName);
            }
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
