using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class UserLayout : ITabLayout
    {
        public string Title => "User";
        public Icon Icon => Icon.CreateUnityIcon("SoftlockInline");

        public void OnGUI()
        {
            var elements = LauncherUserSettings.instance.UserLayout;
            if (elements.Length <= 0)
            {
                EditorGUILayout.HelpBox("You can edit custom your only.", MessageType.Info);
                if (EditorGUIUtil.IconTextButton("d__Popup", "Edit"))
                {
                    SettingsService.OpenUserPreferences(LauncherUserSettingsProvider.SettingPath);
                }
                return;
            }
            foreach (var entry in elements)
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
