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
        public string Tooltip()
        {
            if (string.IsNullOrEmpty(Path.SettingPath))
            {
                return string.Empty;
            }
            return $"Open {Path.SettingPath}";
        }
    }
}
