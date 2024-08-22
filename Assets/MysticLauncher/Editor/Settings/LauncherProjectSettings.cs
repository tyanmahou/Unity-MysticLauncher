using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("ProjectSettings/MysticLauncherProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    public class LauncherProjectSettings : ScriptableSingleton<LauncherProjectSettings>
    {
        [Serializable]
        public struct ProjectInfoType
        {
            public Icon Icon;
            public string ProjectName;
        }
        [SerializeField]
        ProjectInfoType _projectInfo = new ProjectInfoType() {
            Icon = Icon.CreateUnityIcon("d_Profiler.UIDetails@2x"),
            ProjectName = "Mystic Launcher"
        };
        public ProjectInfoType ProjectInfo => _projectInfo;

        public void Save()
        {
            Save(true);
        }
    }
}
