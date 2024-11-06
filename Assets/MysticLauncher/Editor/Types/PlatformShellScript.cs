using System;

namespace Mystic
{
    [Serializable]
    public class PlatformShellScript
    {
        public string Windows = string.Empty;
        public string OSX = string.Empty;

        public bool AutoPause = true;
        [FolderSelect]
        public string WorkingDirectory = string.Empty;
    }
}
