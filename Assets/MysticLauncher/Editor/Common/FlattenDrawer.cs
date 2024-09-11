using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(FlattenAttribute))]
    public class FlattenDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty it = property.Copy();
            SerializedProperty end = it.GetEndProperty();
            position.height = EditorGUIUtility.singleLineHeight;
            while (it.NextVisible(true) && !SerializedProperty.EqualContents(it, end))
            {
                EditorGUI.PropertyField(position, it, true);
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float h = 0;
            SerializedProperty it = property.Copy();
            SerializedProperty end = it.GetEndProperty();
            while (it.NextVisible(true) && !SerializedProperty.EqualContents(it, end))
            {
                h += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            return h;
        }
    }
}
