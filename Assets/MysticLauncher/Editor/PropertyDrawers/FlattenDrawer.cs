using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(FlattenAttribute))]
    public class FlattenDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var propScoped = new EditorGUI.PropertyScope(position, label, property);

            SerializedProperty it = property.Copy();
            SerializedProperty end = it.GetEndProperty();
            position.height = EditorGUIUtility.singleLineHeight;

            if (it.NextVisible(true) && !SerializedProperty.EqualContents(it, end))
            {
                EditorGUI.PropertyField(position, it, true);
                position.y += EditorGUI.GetPropertyHeight(it, true) + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                EditorGUI.PropertyField(position, property, true);
                return;
            }

            while (it.NextVisible(false) && !SerializedProperty.EqualContents(it, end))
            {
                EditorGUI.PropertyField(position, it, true);
                position.y += EditorGUI.GetPropertyHeight(it, true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float h = 0;
            SerializedProperty it = property.Copy();
            SerializedProperty end = it.GetEndProperty();
            if (it.NextVisible(true) && !SerializedProperty.EqualContents(it, end))
            {
                h += EditorGUI.GetPropertyHeight(it, true);
                h += EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property, true);
            }
            while (it.NextVisible(false) && !SerializedProperty.EqualContents(it, end))
            {
                h += EditorGUI.GetPropertyHeight(it, true);
                h += EditorGUIUtility.standardVerticalSpacing;
            }
            return h;
        }
    }
}
