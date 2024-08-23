using UnityEditor;
using UnityEngine;

namespace Mystic
{

    [CustomPropertyDrawer(typeof(MenuItemPickerAttribute))]
    public class MenuItemPickerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            MenuItemPickerAttribute attr = attribute as MenuItemPickerAttribute;

            bool enabledOld = GUI.enabled;
            // ラベルを描画
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width -= 30;
            GUI.enabled = enabledOld && attr.FreeInput;
            property.stringValue = EditorGUI.TextField(position, property.stringValue);
            if (_icon == null)
            {
                _icon = EditorGUIUtility.IconContent("Search On Icon");
            }
            GUI.enabled = enabledOld && true;
            position.x += position.width;
            position.width = 30;
            if (GUI.Button(position, _icon))
            {
                MenuItemPickerWindow.Show(property);
            }
            GUI.enabled = enabledOld;
            EditorGUI.EndProperty();
        }

        GUIContent _icon;
    }

}
