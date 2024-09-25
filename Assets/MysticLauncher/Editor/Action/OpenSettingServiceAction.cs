using System;

namespace Mystic
{
    [Serializable]
    public class OpenSettingServiceAction : IToolAction
    {
        public SettingServicePath Path;

        public void Execute()
        {
            Path.Open();
        }
    }
}
