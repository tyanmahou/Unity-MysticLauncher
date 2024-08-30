using System;
using System.Collections.Generic;

namespace Mystic
{
    [Serializable]
    public class FavoriteList
    {
        [NamedArrayElement]
        public List<FavoriteEntry> Entries = new();

        public bool IsRegistered(UnityEngine.Object asset)
        {
            return Find(asset) != null;
        }
        public FavoriteEntry Find(UnityEngine.Object asset)
        {
            return Entries.Find(f => f.Asset == asset);
        }
        public void Register(UnityEngine.Object asset, string group)
        {
            Entries.Add(new FavoriteEntry()
            {
                Asset = asset,
                FavoriteGroup = group,
            });
        }
        public void Replace(UnityEngine.Object asset, string group)
        {
            var find = Find(asset);
            if (find != null)
            {
                find.FavoriteGroup = group;
            }
            else
            {
                Register(asset, group);
            }
        }
        public void Unregister(UnityEngine.Object asset)
        {
            Entries.RemoveAll(f => f.Asset == asset);
        }
        public void Unregister(FavoriteEntry entry)
        {
            Entries.Remove(entry);
        }
    }
}
