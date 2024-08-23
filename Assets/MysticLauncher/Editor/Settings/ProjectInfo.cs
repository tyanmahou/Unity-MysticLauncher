using System;

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
                }
            };
        }
        public Label ProjectName;
    }
}
