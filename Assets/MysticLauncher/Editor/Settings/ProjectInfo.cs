using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public struct ProjectInfo
    {
        public static ProjectInfo Default()
        {
            return new ProjectInfo
            {
                ProjectName = new Label()
                {
                    Text = "Mystic Launcher",
                    Icon = Icon.CreateUnityIcon("d_Profiler.UIDetails@2x"),
                },
                HelpUrl = "https://github.com/tyanmahou/Unity-MysticLauncher",
            };
        }
        public Label ProjectName;
        public string HelpUrl;

        [SerializeReference, SubclassSelector]

        public IElement CustomHeader;
    }
}
