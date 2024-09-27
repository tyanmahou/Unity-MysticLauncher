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
    }
}
