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

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
            {
                try
                {
                    var path = PathUtil.RelativeOrFullPath(FileName);
                    using Process process = System.Diagnostics.Process.Start(path);
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
