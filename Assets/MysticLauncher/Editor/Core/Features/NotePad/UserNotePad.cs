using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [FilePath("UserSettings/MysticUserNotePad.asset", FilePathAttribute.Location.ProjectFolder)]
    public class UserNotePad : ScriptableSingleton<UserNotePad>
    {
        [SerializeField, HideInInspector]
        private List<MemoEntry> _memos = new List<MemoEntry>();
        public IEnumerable<MemoEntry> EnumerateMemos => _memos;

        public int Count => _memos.Count;
        [HideInInspector]
        public int SelectIndex = -1;

        public MemoEntry SelectedMemo => Memo(SelectIndex);
        public MemoEntry Memo(int index)
        {
            if (0 <= index && index < _memos.Count)
            {
                return _memos[index];
            }
            return null;
        }
        public void Register(MemoEntry entry)
        {
            _memos.Add(entry);
        }
        public void Unregister(MemoEntry entry)
        {
            _memos.Remove(entry);
        }
        public void Clear()
        {
            _memos.Clear();
        }
        public void Save()
        {
            Save(true);
        }
        private void OnValidate()
        {
            if (!EditorUtility.IsPersistent(this))
            {
                Save(true);
            }
        }
    }
}
