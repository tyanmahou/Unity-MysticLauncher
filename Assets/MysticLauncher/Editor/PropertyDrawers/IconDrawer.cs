using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(Icon))]
    public class IconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            position.width = 32;
            position.height = 32;
            var icon = property.FindPropertyRelative("_icon");
            var unityIcon = property.FindPropertyRelative("_unityIcon");
            var emoji = property.FindPropertyRelative("_emoji");

            GUIContent content = GUIContent.none;
            if (icon.objectReferenceValue != null)
            {
                content = new GUIContent(icon.objectReferenceValue as Texture);
            }
            else if (!string.IsNullOrEmpty(unityIcon.stringValue))
            {
                content = EditorGUIUtility.IconContent(unityIcon.stringValue);
            }
            else if (!string.IsNullOrEmpty(emoji.stringValue))
            {
                content = new GUIContent(EmojiUtil.FromUnicodeKey(emoji.stringValue));
            }
            if (GUI.Button(position, content))
            {
                IconPickerWindow.Show(unityIcon, emoji, icon);
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
