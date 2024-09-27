using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticLauncherUserSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherUserSettings : ScriptableSingleton<LauncherUserSettings>
    {
        [NamedArrayElement, SerializeReference, SubclassSelector]

        public IElement[] UserLayout = new IElement[0];

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public ITabLayout[] UserTabs = new ITabLayout[]
        {
            new UserLayout(),
            new FavoriteLayout(),
            new HistoryLayout(),
        };

        public void OnValidate()
        {
            if (!EditorUtility.IsPersistent(this))
            {
                Save(true);
            }

        }
        public void Save()
        {
            Save(true);
        }
    }
}
