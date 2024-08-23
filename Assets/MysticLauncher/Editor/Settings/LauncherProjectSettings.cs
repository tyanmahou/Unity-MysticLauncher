using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("ProjectSettings/MysticLauncherProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherProjectSettings : ScriptableSingleton<LauncherProjectSettings>
    {
        [SerializeField]
        ProjectInfo _projectInfo = ProjectInfo.Default();
        public ProjectInfo ProjectInfo => _projectInfo;

        public PortalLayout Portal;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public ITabLayout[] CustomTabs;

        public void Save()
        {
            Save(true);
        }
    }
}
