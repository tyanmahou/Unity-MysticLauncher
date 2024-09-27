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
            bool old = GUI.enabled;
            using var horizontal = new EditorGUILayout.HorizontalScope();

            var skin = new GUIStyle(EditorStyles.objectField);
            skin.richText = true;
            var content = Label.GetGUIContent();
            string replacedPath = PathUtil.ReplaceEnv(Path);
            if (string.IsNullOrEmpty(content.text))
            {
                content.text = replacedPath;
            }
            else
            {
                content.text = $"{Label.Text} <color=grey>({replacedPath})</color>";
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
                    OpenTerminal(path);
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
        // ターミナル開く
        void OpenTerminal(string path)
        {
            var terminalPath = UserEnv.instance.TerminalPath;
            if (!File.Exists(terminalPath))
            {
                terminalPath = string.Empty;
            }
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = terminalPath,
                WorkingDirectory = path
            };
            try
            {
                using Process process = Process.Start(processInfo);
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
