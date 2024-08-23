using System;
using System.Diagnostics;

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
                    using Process process = System.Diagnostics.Process.Start(FileName);
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
