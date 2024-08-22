using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// アイコン
    /// </summary>
    [Serializable]
    public class Icon
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

        public bool IsValid
        {
            get
            {
                return _icon != null || !string.IsNullOrEmpty(_unityIcon);
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
                icon = EditorGUIUtility.IconContent(_unityIcon);
                return true;
            }
            icon = null;
            return false;
        }
    }
}
