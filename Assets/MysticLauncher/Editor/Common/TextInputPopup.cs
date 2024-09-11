using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class TextInputPopup : PopupWindowContent
    {
        public static void Show(Rect position, SerializedProperty prop)
        {
            Rect pos = position;
            pos.height = 0;
            pos.width = 0;
            PopupWindow.Show(pos, new TextInputPopup(position, prop));
        }
        TextInputPopup(Rect position, SerializedProperty prop)
        {
            _position = position;
            _prop = prop;
        }

        public override Vector2 GetWindowSize()
        {
            return _position.size;
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
        Rect _position;
    }
}
