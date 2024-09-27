//using UnityEditor;
//using UnityEngine;

//namespace Mystic
//{

//    [CustomPropertyDrawer(typeof(EnvVariable))]
//    public class EnvVariableDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            int i = EditorGUI.indentLevel;
//            EditorGUI.BeginProperty(position, label, property);
//            EditorGUI.indentLevel = 0;
//            var variable = property.FindPropertyRelative("Variable");
//            var value = property.FindPropertyRelative("Value");

//            var variablePos = position;
//            variablePos.width = position.width / 2 - 4;
//            variable.stringValue = EditorGUI.TextField(variablePos, variable.stringValue);

//            var valuePos = variablePos;
//            valuePos.x += position.width / 2;
//            value.stringValue = EditorGUI.TextField(valuePos, value.stringValue);
//            EditorGUI.EndProperty();
//            EditorGUI.indentLevel = i;
//        }

//        GUIContent _icon;
//    }

//}
