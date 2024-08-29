using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticLauncherUserSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherUserSettings : ScriptableSingleton<LauncherUserSettings>
    {
        [SerializeField, FileSelect]
        private string _terminalPath = "";
        public string TerminalPath => _terminalPath;

        [SerializeField]
        private EnvSettings _env;
        public EnvSettings Env => _env;

        [NamedArrayElement, SerializeReference, SubclassSelector]

        public IElement[] UserLayout = new IElement[0];

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public ITabLayout[] UserTabs = new ITabLayout[2]
        {
            new UserLayout(),
            new FavoriteLayout()
        };

        public FavoriteList Favorite = new();

        public void Save()
        {
            Save(true);
        }
    }
}
