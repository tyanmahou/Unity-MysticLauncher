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
        public static void Exec(PlatformShellScript script)
        {
            // スクリプトファイルに内容を書き込む
#if UNITY_EDITOR_WIN
            string scriptSourceCode = script.Windows;
#else
            string scriptSourceCode = script.OSX;
#endif
            if (script.AutoPause)
            {
#if UNITY_EDITOR_WIN
               scriptSourceCode += "\npause";
#else
               scriptSourceCode += "\nread";
#endif
            }
            try
            {
                string workingDir = string.IsNullOrEmpty(script.WorkingDirectory) ? string.Empty : PathUtil.FixedFullPath(script.WorkingDirectory);
                // PowerShell スクリプトを実行
#if UNITY_EDITOR_WIN
                var processInfo = new ProcessStartInfo
                {
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{scriptSourceCode}\"",
                    WorkingDirectory = workingDir
                };
#else
                var processInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{scriptSourceCode}\"",
                    WorkingDirectory = workingDir,
                };
#endif
                using Process process = Process.Start(processInfo);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
    }
}
