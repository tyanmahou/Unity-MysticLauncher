using System;
using System.Diagnostics;
using System.IO;

namespace Mystic
{
    [Serializable]
    public class ProcessElement : IElement
    {
        public Label Label;

        [FileSelect]
        public string FileName;

        [FolderSelect]
        public string WorkingDirectory;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
            {
                try
                {
                    var fileName = PathUtil.RelativeOrFullPath(FileName);
                    var workingDir = string.IsNullOrEmpty(WorkingDirectory) ? string.Empty : PathUtil.RelativeOrFullPath(WorkingDirectory);
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
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
