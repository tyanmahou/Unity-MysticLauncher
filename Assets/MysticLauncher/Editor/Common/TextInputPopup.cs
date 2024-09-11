using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class TextInputPopup : PopupWindowContent
    {
        public TextInputPopup(SerializedProperty prop, Vector2 size)
        {
            _prop = prop;
            _size = size;
        }

        public override Vector2 GetWindowSize()
        {
            return _size;
        }
        public override void OnGUI(Rect rect)
        {
            GUILayout.Label(_prop.name, EditorStyles.boldLabel);
            var prev = _prop.stringValue;
            _prop.stringValue = EditorGUILayout.TextField(_prop.stringValue);            
            if (prev != _prop.stringValue)
            {
                _prop.serializedObject.ApplyModifiedProperties();
            }
        }
        SerializedProperty _prop;
        Vector2 _size;
    }
}
