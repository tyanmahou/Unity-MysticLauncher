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
        public static SettingServicePath Create(SettingsScope scope, string path)
        {
            return new SettingServicePath()
            {
                Scope = scope,
                SettingPath = path,
            };
        }
        public static SettingServicePath CreateProject(string path) => Create(SettingsScope.Project, path);
        public static SettingServicePath CreateUser(string path) => Create(SettingsScope.User, path);

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
