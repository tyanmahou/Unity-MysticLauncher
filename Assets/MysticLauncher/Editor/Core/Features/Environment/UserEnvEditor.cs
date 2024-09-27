using System;
using System.Diagnostics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mystic
{
    [CustomEditor(typeof(UserEnv))]
    public class UserEnvEditor : Editor
    {
        public void OnEnable()
        {
            _terminalPath = serializedObject.FindProperty("_terminalPath");
            _variables = serializedObject.FindProperty("_variables");

            _reorderableVariables = new ReorderableList(_variables.serializedObject, _variables, true, true, true, true)
            {
                drawElementCallback = DrawVariableElement,
                drawHeaderCallback = DrawVariableHeader,
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_terminalPath);
            EditorGUILayout.Space(30);
            EditorGUIUtil.DrawSeparator();
            using (new GUILayout.HorizontalScope())
            {
                _toggleVariables = EditorGUILayout.Foldout(_toggleVariables, "Variables");

#if UNITY_EDITOR_WIN
                GUILayout.FlexibleSpace();
                if (EditorGUIUtil.IconTextButton("d__Popup", "PC", "Open PC Environment Variables Edit"))
                {
                    Process.Start("rundll32.exe", "sysdm.cpl,EditEnvironmentVariables");
                }
#endif
            }
            GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
            if (_toggleVariables)
            {
                var rect = EditorGUILayout.GetControlRect();
                rect.x += 15;
                rect.width -= 15;
                _reorderableVariables.DoList(rect);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.EndChangeCheck();
        }

        private void DrawVariableHeader(Rect rect)
        {
            const float offset = 18;
            rect.x += offset;
            rect.width -= offset;
            var variablePos = rect;
            variablePos.width = rect.width / 2 - 4;
            EditorGUI.LabelField(variablePos, "Variable");

            var valuePos = variablePos;
            valuePos.x += rect.width / 2;
            EditorGUI.LabelField(valuePos, "Value");
        }
        private void DrawVariableElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            const float margin = 4;
            rect.height -= margin;
            rect.y += margin / 2;
            var element = _reorderableVariables.serializedProperty.GetArrayElementAtIndex(index);
            var variable = element.FindPropertyRelative("Variable");
            var value = element.FindPropertyRelative("Value");

            var variablePos = rect;
            variablePos.width = rect.width / 2 - 4;
            variable.stringValue = EditorGUI.TextField(variablePos, variable.stringValue);

            var valuePos = variablePos;
            valuePos.x += rect.width / 2;
            value.stringValue = EditorGUI.TextField(valuePos, value.stringValue);
        }
        SerializedProperty _terminalPath;
        SerializedProperty _variables;
        private bool _toggleVariables = true;
        private ReorderableList _reorderableVariables;
    }
}
