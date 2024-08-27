using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class UserLayout : ITabLayout
    {
        [NamedArrayElement, SerializeReference, SubclassSelector]

        public IElement[] Elements = new IElement[0];

        public string Title => "User";
        public Icon Icon => Icon.CreateUnityIcon("SoftlockInline");

        public void OnGUI()
        {
            if (Elements.Length <= 0)
            {
                var icon = new GUIContent(EditorGUIUtility.IconContent("d__Popup"));
                icon.text = "Edit";
                if (GUILayout.Button(icon))
                {
                    SettingsService.OpenUserPreferences(LauncherUserSettingsProvider.SettingPath);
                }
                return;
            }
            foreach (var entry in Elements)
            {
                entry?.OnGUI();
            }
        }
        public override string ToString()
        {
            return Title;
        }
    }
}
