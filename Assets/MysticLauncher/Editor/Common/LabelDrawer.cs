using System.Diagnostics;
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
            var popPos = position;
            popPos.height = EditorGUIUtility.singleLineHeight * 2 + 5;
            popPos.y -= popPos.height - EditorGUIUtility.singleLineHeight;
            popPos.width -= 32;
            EditorGUI.indentLevel = 0;
            var text = property.FindPropertyRelative("Text");
            var tooltip = property.FindPropertyRelative("Tooltip");
            var icon = property.FindPropertyRelative("Icon");
            position.width -= 32 + 20 + 2;
            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, text, GUIContent.none);
            position.x += position.width + 2;
            position.width = 20;
            if (GUI.Button(position, EditorGUIUtility.IconContent("Info"), EditorStyles.iconButton))
            {
                PopupWindow.Show(popPos, new TextInputPopup(tooltip, popPos.size));
            }
            position.x += 20;
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
