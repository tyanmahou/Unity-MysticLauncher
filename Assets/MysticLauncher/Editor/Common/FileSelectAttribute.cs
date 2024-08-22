using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// ファイル選択用属性
    /// </summary>
    public class FileSelectAttribute : PropertyAttribute
    {
        public FileSelectAttribute(
            string title = "Select a File",
            string diectory = "",
            string extension = "",
            bool freeInput = true
            )
        {
            this.Title = title;
            this.Directory = diectory;
            this.Extension = extension;
            this.FreeInput = freeInput;
        }
        public string Title;
        public string Directory;
        public string Extension;
        public bool FreeInput;
    }
}
