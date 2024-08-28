using System;

namespace Mystic
{
    [Serializable]
    public class FavoriteEntry
    {
        public UnityEngine.Object Asset;
        public string FavoriteGroup;

        public override string ToString()
        {
            var path = FavoriteGroup;
            if (Asset != null) {
                if (string.IsNullOrEmpty(path)) 
                {
                    path += Asset.name;
                }
                else
                {
                    path += "/" + Asset.name;
                }
            }
            return path;
        }
    }
}
