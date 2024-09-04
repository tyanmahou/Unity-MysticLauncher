using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(UnityIconAttribute))]
    public class UnityIconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            position.width -= 32;
            EditorGUI.LabelField(position, string.IsNullOrEmpty(property.stringValue) ? "None" : property.stringValue);
            position.x += position.width;
            position.width = 32;
            position.height = 32;

            bool onShowPicker = string.IsNullOrEmpty(property.stringValue) ? GUI.Button(position, "")
                : GUI.Button(position, EditorGUIUtility.IconContent(property.stringValue));
            if (onShowPicker)
            {
                UnityIconPickerWindow.Show(property);
            }

            EditorGUI.EndProperty();
            EditorGUI.indentLevel = i;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
