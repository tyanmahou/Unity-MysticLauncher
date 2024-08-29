using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class OpenUrlAction : IToolAction
    {
        public string Url;

        public void Execute()
        {
            Application.OpenURL(Url);
        }
    }
}
