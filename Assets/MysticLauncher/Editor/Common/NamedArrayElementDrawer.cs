using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [CustomPropertyDrawer(typeof(NamedArrayElementAttribute))]
    public class NamedArrayElementDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            try
            {
                int _ = int.Parse(property.propertyPath.Split('[', ']')[1]);
                var name = GetTitle(property) ?? label.text;
                EditorGUI.PropertyField(position, property, new GUIContent(name), true);
            }
            catch
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, true);
        }
        string GetTitle(SerializedProperty prop)
        {
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Generic:
                    return prop.boxedValue?.ToString();
                case SerializedPropertyType.Integer:
                    return prop.intValue.ToString();
                case SerializedPropertyType.Boolean:
                    return prop.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return prop.floatValue.ToString("G");
                case SerializedPropertyType.String:
                    return prop.stringValue;
                case SerializedPropertyType.Color:
                    return prop.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return prop.objectReferenceValue.ToString();
                case SerializedPropertyType.LayerMask:
                    break;
                case SerializedPropertyType.Enum:
                    return prop.enumNames[prop.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return prop.vector2Value.ToString();
                case SerializedPropertyType.Vector3:
                    return prop.vector3Value.ToString();
                case SerializedPropertyType.Vector4:
                    return prop.vector4Value.ToString();
                case SerializedPropertyType.ManagedReference:
                    return prop.managedReferenceValue.ToString();
            }

            return null;
        }
    }
}
