using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(Label))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var text = property.FindPropertyRelative("Text");
            var icon = property.FindPropertyRelative("Icon");

            position.x -= EditorGUI.indentLevel * 15;
            position.width -= 32 - EditorGUI.indentLevel * 15 + 20;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, text, GUIContent.none);
            position.x += position.width;
            position.width = 32;
            EditorGUI.PropertyField(position, icon, GUIContent.none);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
