using System;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// アイコン
    /// </summary>
    [Serializable]
    public class Icon
    {
        [SerializeField] Texture _icon;
        [SerializeField] string _unityIcon;
    }
}
