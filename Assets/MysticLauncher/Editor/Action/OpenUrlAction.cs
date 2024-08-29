using System;
using System.Diagnostics;

namespace Mystic
{
    [Serializable]
    public class OpenUrlAction : IToolAction
    {
        public string Url;

        public void Execute()
        {
            try
            {
                using Process process = System.Diagnostics.Process.Start(Url);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
    }
}
