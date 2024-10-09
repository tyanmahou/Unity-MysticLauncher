using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class OpenURLAction : IToolAction
    {
        public string URL;

        public void Execute()
        {
            Application.OpenURL(URL);
        }
        public string Tooltip()
        {
            if (string.IsNullOrEmpty(URL))
            {
                return string.Empty;
            }
            return $"Open URL\n<color=grey>{URL}</color>";
        }
    }
}
