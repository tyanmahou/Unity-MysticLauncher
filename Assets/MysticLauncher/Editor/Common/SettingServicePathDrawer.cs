using UnityEditor;
using UnityEngine;

namespace Mystic
{

    [CustomPropertyDrawer(typeof(SettingServicePath))]
    public class SettingServicePathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            var scope = property.FindPropertyRelative("Scope");
            var path = property.FindPropertyRelative("SettingPath");

            EditorGUI.BeginProperty(position, label, property);
            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
            float w = position.width;
            position.width = 65;
            scope.enumValueIndex = (int)(SettingsScope)EditorGUI.EnumPopup(position, (SettingsScope)scope.enumValueIndex);
            position.x += position.width;

            position.width = w - 95;
            path.stringValue = EditorGUI.TextField(position, path.stringValue);
            position.x += position.width;

            position.width = 30;
            if (GUI.Button(position, EditorGUIUtility.IconContent("Search On Icon")))
            {
            }
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = i;
        }
    }

}
