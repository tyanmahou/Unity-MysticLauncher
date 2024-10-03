using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public struct EmojiData
    {
        public string unified;
        public string short_name;
        public List<string> short_names;
    }

    public class EmojiDataList : ScriptableObject
    {
        public IReadOnlyList<EmojiData> Emojis => _emojis;

        [SerializeField]
        private List<EmojiData> _emojis;

        public EmojiData Find(string unicodeKey)
        {
            TryFind(unicodeKey, out var emoji);
            return emoji;
        }
        public bool TryFind(string unicodeKey, out EmojiData emoji)
        {
            UpdateLookupTable();
            if (_lookupTable.TryGetValue(unicodeKey, out var index))
            {
                emoji = _emojis[index];
                return true;
            }
            emoji = default;
            return false;
        }
        public string GetShortName(string unicodeKey)
        {
            return Find(unicodeKey).short_name ?? string.Empty;
        }
        public bool IsSearched(string unicodeKey, string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return true;
            }
            if (TryFind(unicodeKey, out var emoji))
            {
                if (!string.IsNullOrEmpty(emoji.short_name))
                {
                    if (emoji.short_name.IsSearched(search))
                    {
                        return true;
                    }
                }
                if (emoji.short_names != null)
                {
                    foreach (var shortName in emoji.short_names)
                    {
                        if (!string.IsNullOrEmpty(shortName))
                        {
                            if (shortName.IsSearched(search))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        void UpdateLookupTable()
        {
            if (_lookupTable != null)
            {
                return;
            }
            _lookupTable = new();
            for (int i = 0; i < _emojis.Count; ++i)
            {
                string key = _emojis[i].unified.ToLower();
                _lookupTable.TryAdd(key, i);
            }
        }
        private Dictionary<string, int> _lookupTable;
    }
}
