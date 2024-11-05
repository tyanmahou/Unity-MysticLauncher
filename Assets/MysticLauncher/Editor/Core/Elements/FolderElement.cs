using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class FolderElement : IElement
    {
        public Label Label;

        [FolderSelect]
        public string Path;

        public void OnGUI()
        {
            var width = EditorGUIUtil.GetIndentedWidth() - 28 * 2;

            bool old = GUI.enabled;
            using var horizontal = new EditorGUILayout.HorizontalScope();

            var skin = new GUIStyle(EditorStyles.textField);
            skin.richText = true;
            skin.imagePosition = ImagePosition.ImageLeft;
            var content = EditorGUIUtil.GetIconContent16x16(Label);
            string replacedPath = PathUtil.ReplaceEnv(Path);
            if (string.IsNullOrEmpty(content.text))
            {
                content.text = replacedPath;
                if (EditorGUIUtil.TruncateFit(content, width, skin))
                {

                }
            }
            else
            {
                content.text = $"{Label.Text} ({replacedPath})";
                if (EditorGUIUtil.TruncateFit(content, width, skin))
                {
                    if (content.text.Length > Label.Text.Length + 4)
                    {
                        content.text = $"{Label.Text} <color=grey>({content.text[(Label.Text.Length + 4)..]})</color>";
                    }
                }
                else
                {
                    content.text = $"{Label.Text} <color=grey>({replacedPath})</color>";
                }
            }
            if (!string.IsNullOrEmpty(content.tooltip))
            {
                content.tooltip += "\n";
            }
            var path = PathUtil.FixedFullPath(Path);
            content.tooltip += $"<color=grey>{path}</color>";
            EditorGUILayout.LabelField(content, skin, GUILayout.MinWidth(0));
            GUI.enabled = old && System.IO.Directory.Exists(path);
            // フォルダを開く
            {
                if (EditorGUIUtil.IconButton("d_FolderOpened Icon", "Open Explorer"))
                {
                    OpenFolder(path);
                }
            }
            // ターミナルを開く
            {
                if (EditorGUIUtil.IconButton("d_BuildSettings.Standalone", "Open Terminal"))
                {
                    TerminalUtil.Open(path);
                }
            }
            GUI.enabled = old;
        }
        void OpenFolder(string path)
        {
            try
            {
                using Process process = System.Diagnostics.Process.Start(path);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
        public override string ToString()
        {
            return Path;
        }
    }
}
