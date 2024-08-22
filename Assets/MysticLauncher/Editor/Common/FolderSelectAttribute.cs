using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// フォルダ選択用属性
    /// </summary>
    public class FolderSelectAttribute : PropertyAttribute
    {
        public FolderSelectAttribute(
            string title = "Select a Folder",
            string folder = "",
            string defaultName = "",
            bool freeInput = true
            )
        {
            this.Title = title;
            this.Folder = folder;
            this.DefaultName = defaultName;
            this.FreeInput = freeInput;
        }
        public string Title;
        public string Folder;
        public string DefaultName;
        public bool FreeInput;
    }
}
