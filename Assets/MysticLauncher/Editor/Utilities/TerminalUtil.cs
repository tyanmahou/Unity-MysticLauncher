using System.Diagnostics;
using System.IO;

namespace Mystic
{
    public static class TerminalUtil
    {
        public static void Open(string path)
        {
            var terminalPath = UserEnv.instance.TerminalPath;
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = terminalPath,
                WorkingDirectory = path
            };
            if (!File.Exists(terminalPath))
            {
#if UNITY_EDITOR_WIN
                // cmdよりpowershellが良さそう
                processInfo.FileName = "powershell.exe";
#elif UNITY_EDITOR_OSX
                processInfo.FileName = "open";
                processInfo.Arguments = "-a Terminal";
#elif UNITY_EDITOR_LINUX
                processInfo.FileName = "gnome-terminal";
#endif
            }
            try
            {
                using Process process = Process.Start(processInfo);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
    }
}
