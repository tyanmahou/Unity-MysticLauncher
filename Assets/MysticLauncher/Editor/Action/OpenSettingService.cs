using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class OpenSettingService : IToolAction
    {
        public SettingsScope Scope;
        public string SettingPath;

        public void Execute()
        {
            if (Scope == SettingsScope.Project)
            {
                SettingsService.OpenProjectSettings(SettingPath);
            }
            else
            {
                SettingsService.OpenUserPreferences(SettingPath);
            }
        }
    }
}
