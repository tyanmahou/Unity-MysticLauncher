using System;
using System.Collections.Generic;

namespace Mystic
{
    [Serializable]
    public class MemoEntry
    {
        public MemoEntry()
        {
            Title = "New Memo";
            Icon = Icon.CreateUnityIcon("d_TextScriptImporter Icon");
            CreatedAt = UpdatedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public Icon Icon;
        public string Title = string.Empty;
        public string Text = string.Empty;
        public string URL = string.Empty;
        public List<UnityEngine.Object> Assets;
        public string CreatedAt = string.Empty;
        public string UpdatedAt = string.Empty;
        public void Set(MemoEntry other)
        {
            Icon = other.Icon;
            Title = other.Title;
            Text = other.Text;
            URL = other.URL;
            Assets = other.Assets;
            CreatedAt = other.CreatedAt;
            UpdatedAt = other.UpdatedAt;
        }
        public void Update(MemoEntry other)
        {
            other.UpdateOpenedAt();
            Set(other);
        }
        public void UpdateOpenedAt()
        {
            UpdatedAt = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }
}
