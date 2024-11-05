using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(PlatformShell))]
    public class PlatformShellDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int i = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(position, label, property);

            // プロパティのラベルを表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            EditorGUI.indentLevel = 0;
#if UNITY_EDITOR_WIN
            var script = property.FindPropertyRelative("Windows");
#else
            var script = property.FindPropertyRelative("OSX");
#endif
            var content = EditorGUIUtil.NewIconContent("d_TextScriptImporter Icon", script.stringValue.Split('\n')[0], script.stringValue);
            var style = new GUIStyle(EditorStyles.textField);
            style.imagePosition = ImagePosition.ImageLeft;
            if (GUI.Button(position, content, style))
            {
                ShellEditWindow.Show(property);
            }
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = i;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 18;
        }
    }
}
