using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticUserHistories.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UserHistories : ScriptableSingleton<UserHistories>
    {
        [SerializeField]
        private int _historyMaxCount = 100;

        [SerializeField, HideInInspector]
        private List<HistoryEntry> _histories = new List<HistoryEntry>();
        public IEnumerable<HistoryEntry> EnumerateHistories => _histories.Reverse<HistoryEntry>();

        public void Register(UnityEngine.Object asset)
        {
            var find = _histories.Find(h => h.Asset == asset);
            if (find != null)
            {
                find.UpdateOpenedAt();
                _histories.Remove(find);
                _histories.Add(find);
                return;
            }
            _histories.Add(new HistoryEntry(asset));
            while (_histories.Count > _historyMaxCount)
            {
                _histories.RemoveAt(0);
            }
        }
        public void Unregister(HistoryEntry entry)
        {
            _histories.Remove(entry);
        }
        public void Save()
        {
            Save(true);
        }
        private void OnValidate()
        {
            if (_historyMaxCount < 0)
            {
                _historyMaxCount = 0;
            }
        }
    }
}
