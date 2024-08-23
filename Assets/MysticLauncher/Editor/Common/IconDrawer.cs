using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(Icon))]
    public class IconDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            bool enabledOld = GUI.enabled;
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var icon = property.FindPropertyRelative("_icon");
            var unityIcon = property.FindPropertyRelative("_unityIcon");

            position.x -= EditorGUI.indentLevel * 15;
            position.width -= 32 - EditorGUI.indentLevel * 15;
            position.height = 32;
            GUI.enabled = enabledOld && string.IsNullOrEmpty(unityIcon.stringValue);
            EditorGUI.ObjectField(position, icon, GUIContent.none);
            position.x += position.width;
            position.width = 32;
            GUI.enabled = enabledOld && icon.objectReferenceValue == null;
            bool onShowPicker = string.IsNullOrEmpty(unityIcon.stringValue) ? GUI.Button(position, "")
                : GUI.Button(position, EditorGUIUtility.IconContent(unityIcon.stringValue));
            if (onShowPicker)
            {
                UnityIconPickerWindow.Show(unityIcon);
            }

            EditorGUI.EndProperty();
            GUI.enabled = enabledOld;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 32;
        }
    }
}
