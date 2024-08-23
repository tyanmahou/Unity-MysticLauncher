using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class FavoriteLayout : ITabLayout
    {
        public string Title => "Favorite";
        public Icon Icon { get; set; } = Icon.CreateUnityIcon("d_Favorite_colored");

        public void OnGUI()
        {

        }
    }
}
