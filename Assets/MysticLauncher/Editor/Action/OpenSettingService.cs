using System;

namespace Mystic
{
    [Serializable]
    public class OpenSettingService : IToolAction
    {
        public SettingServicePath Path;

        public void Execute()
        {
            Path.Open();
        }
    }
}
