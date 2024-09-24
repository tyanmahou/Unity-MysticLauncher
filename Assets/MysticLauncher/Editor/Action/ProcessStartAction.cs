using System;
using System.Diagnostics;

namespace Mystic
{
    [Serializable]
    public class ProcessStartAction : IToolAction
    {
        [FileSelect]
        public string FileName;

        [FolderSelect]
        public string WorkingDirectory;

        public void Execute()
        {
            try
            {
                var fileName = PathUtil.FixedFullPath(FileName);
                var workingDir = string.IsNullOrEmpty(WorkingDirectory) ? string.Empty : PathUtil.FixedFullPath(WorkingDirectory);
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    WorkingDirectory = workingDir
                };
                using Process process = System.Diagnostics.Process.Start(processInfo);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
    }
}
