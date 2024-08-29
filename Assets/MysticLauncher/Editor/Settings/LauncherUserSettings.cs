using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticLauncherUserSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherUserSettings : ScriptableSingleton<LauncherUserSettings>
    {
        [SerializeField, FileSelect]
        private string _terminalPath = "";

        [SerializeField]
        private EnvSettings _env;
        public EnvSettings Env => _env;

        public string TerminalPath => _terminalPath;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public ITabLayout[] UserTabs = new ITabLayout[1]
        {
            new UserLayout()
        };

        public FavoriteList Favorite = new();

        public void Save()
        {
            Save(true);
        }
    }
}
