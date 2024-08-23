using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(Icon))]
    public class IconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            position.width = 32;
            position.height = 32;
            var icon = property.FindPropertyRelative("_icon");
            var unityIcon = property.FindPropertyRelative("_unityIcon");
            GUIContent content = GUIContent.none;
            if (icon.objectReferenceValue != null)
            {
                content = new GUIContent(icon.objectReferenceValue as Texture);
            }
            else if (!string.IsNullOrEmpty(unityIcon.stringValue))
            {
                content = EditorGUIUtility.IconContent(unityIcon.stringValue);
            }
            if (GUI.Button(position, content))
            {
                UnityIconPickerWindow.Show(icon, unityIcon);
            }
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
