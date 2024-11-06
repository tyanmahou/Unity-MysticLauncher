using System;

namespace Mystic
{
    [Serializable]
    public class ShellAction : IToolAction
    {
        public PlatformShellScript Script = new();

        public bool AutoPause = true;

        [FolderSelect]
        public string WorkingDirectory = string.Empty;

        public void Execute()
        {
            var workingDir = string.IsNullOrEmpty(WorkingDirectory) ? string.Empty : PathUtil.FixedFullPath(WorkingDirectory);

            TerminalUtil.Exec(Script, AutoPause, workingDir);
        }
        public string Tooltip()
        {
            return $"Execute Script";
        }
    }
}
