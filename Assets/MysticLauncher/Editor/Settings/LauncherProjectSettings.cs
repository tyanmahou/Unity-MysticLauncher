﻿using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("ProjectSettings/MysticLauncherProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherProjectSettings : ScriptableSingleton<LauncherProjectSettings>
    {
        [SerializeField]
        ProjectInfo _projectInfo = ProjectInfo.Default();
        public ProjectInfo ProjectInfo => _projectInfo;

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public IElement[] PortalLayout = new IElement[0];

        [NamedArrayElement, SerializeReference, SubclassSelector]
        public ITabLayout[] ProjectTabs = new ITabLayout[0];
        public void OnValidate()
        {
            // 変更があったら保存
            Save(true);
        }
        public void Save()
        {
            Save(true);
        }
    }
}
