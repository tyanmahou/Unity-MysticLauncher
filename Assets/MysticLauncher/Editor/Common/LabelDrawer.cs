using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(Label))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            var text = property.FindPropertyRelative("Text");
            var icon = property.FindPropertyRelative("Icon");
            position.width -= 32;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, text, GUIContent.none);
            position.x += position.width;
            position.width = 32;
            EditorGUI.PropertyField(position, icon, GUIContent.none);
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = i;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
