using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// アイコン
    /// </summary>
    [Serializable]
    public struct Icon
    {
        public static Icon CreateUnityIcon(string name)
        {
            return new Icon()
            {
                _unityIcon = name,
            };
        }
        [SerializeField] Texture _icon;
        [SerializeField] string _unityIcon;
        [SerializeField] string _emoji;

        public bool IsValid
        {
            get
            {
                return _icon != null || !string.IsNullOrEmpty(_unityIcon) || !string.IsNullOrEmpty(_emoji);
            }
        }

        public bool TryGetGUIContent(out GUIContent icon)
        {
            if (_icon != null)
            {
                icon = new GUIContent
                {
                    image = _icon
                };
                return true;
            }
            if (!string.IsNullOrEmpty(_unityIcon))
            {
                icon = new GUIContent(EditorGUIUtility.IconContent(_unityIcon));
                return true;
            }
            if (!string.IsNullOrEmpty(_emoji))
            {
                icon = new GUIContent(EmojiUtil.FromUnicodeKey(_emoji));
                return true;
            }
            icon = null;
            return false;
        }
    }
}
