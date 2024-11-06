using System;

namespace Mystic
{
    [Serializable]
    public class ShellAction : IToolAction
    {
        public PlatformShellScript Script = new();

        public void Execute()
        {
            TerminalUtil.Exec(Script);
        }
        public string Tooltip()
        {
            return $"Execute Script";
        }
    }
}
