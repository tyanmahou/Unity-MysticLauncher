using System;
using System.Diagnostics;

namespace Mystic
{
    [Serializable]
    public class OpenFolderAction : IToolAction
    {
        [FolderSelect]
        public string Path;

        public void Execute()
        {
            var path = PathUtil.FixedFullPath(Path);
            try
            {
                using Process process = System.Diagnostics.Process.Start(path);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
        public string Tooltip()
        {
            if (string.IsNullOrEmpty(Path))
            {
                return string.Empty;
            }
            var path = PathUtil.FixedFullPath(Path);
            return $"Open Folder\n<color=grey>{path}</color>";
        }
    }
}
