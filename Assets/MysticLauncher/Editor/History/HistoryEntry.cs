using System;

namespace Mystic
{
    [Serializable]
    public class HistoryEntry
    {
        public HistoryEntry(UnityEngine.Object asset)
        {
            Asset = asset;
            OpenedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public UnityEngine.Object Asset;
        public string OpenedAt;

        public void UpdateOpenedAt()
        {
            OpenedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
