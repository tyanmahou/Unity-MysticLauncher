using System.Diagnostics;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Mystic
{

    public class EnvVariableListDrawer : PropertyDrawer
    {
        private ReorderableList reorderableList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (reorderableList == null)
            {
                var list = property.FindPropertyRelative("Variables");
                reorderableList = new ReorderableList(list.serializedObject, list, true, true, true, true)
                {
                    drawElementCallback = DrawElement,
                    drawHeaderCallback = DrawHeader,
                    //onAddCallback = OnAdd,
                    //onRemoveCallback = OnRemove
                };
            }

            reorderableList.DoList(position);
            if (GUILayout.Button("a"))
            {
                Process.Start("rundll32.exe", "sysdm.cpl,EditEnvironmentVariables");
            }
        }

        private void DrawHeader(Rect rect)
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

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            const float margin = 4;
            rect.height -= margin;
            rect.y += margin / 2;
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var variable = element.FindPropertyRelative("Variable");
            var value = element.FindPropertyRelative("Value");

            var variablePos = rect;
            variablePos.width = rect.width / 2 - 4;
            variable.stringValue = EditorGUI.TextField(variablePos, variable.stringValue);

            var valuePos = variablePos;
            valuePos.x += rect.width / 2;
            value.stringValue = EditorGUI.TextField(valuePos, value.stringValue);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return reorderableList != null ? reorderableList.GetHeight() : base.GetPropertyHeight(property, label);
        }
    }

}
