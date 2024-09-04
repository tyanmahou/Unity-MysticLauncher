using System;
using UnityEditor;

namespace Mystic
{
    /// <summary>
    /// セッティングパス
    /// </summary>
    [Serializable]
    public struct SettingServicePath
    {
        public SettingsScope Scope;
        public string SettingPath;

        public void Open()
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
