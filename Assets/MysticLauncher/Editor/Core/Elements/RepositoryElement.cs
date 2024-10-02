using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class RepositoryElement : IElement
    {
        public static RepositoryElement Create(string text, string localPath, string remotePath = "", string tooltip = "", string icon = null)
        {
            return new RepositoryElement()
            {
                Label = Label.Create(text, tooltip, icon),
                LocalPath = localPath,
                RemoteUrl = remotePath,
            };
        }
        public Label Label;

        [FolderSelect]
        public string LocalPath;

        public string RemoteUrl;

        public void OnGUI()
        {
            var width = EditorGUIUtil.GetIndentedWidth() - 28 * 3;

            bool old = GUI.enabled;
            using var horizontal = new EditorGUILayout.HorizontalScope();
            var skin = new GUIStyle(EditorStyles.textField);
            skin.richText = true;
            skin.imagePosition = ImagePosition.ImageLeft;
            var content = EditorGUIUtil.GetIconContent16x16(Label);
            string replacedLocalPath = PathUtil.ReplaceEnv(LocalPath);
            if (string.IsNullOrEmpty(content.text))
            {
                content.text = replacedLocalPath;
                if (EditorGUIUtil.TruncateFit(content, width, skin))
                {

                }
            }
            else
            {
                content.text = $"{Label.Text} ({replacedLocalPath})";
                if (EditorGUIUtil.TruncateFit(content, width, skin))
                {
                    if (content.text.Length > Label.Text.Length + 4)
                    {
                        content.text = $"{Label.Text} <color=grey>({content.text[(Label.Text.Length + 4)..]})</color>";
                    }
                }
                else
                {
                    content.text = $"{Label.Text} <color=grey>({replacedLocalPath})</color>";
                }
            }
            if (!string.IsNullOrEmpty(content.tooltip))
            {
                content.tooltip += "\n";
            }
            var path = PathUtil.FixedFullPath(LocalPath);
            content.tooltip += $"<color=grey>{path}</color>";
            EditorGUILayout.LabelField(content, skin, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight));

            GUI.enabled = old && System.IO.Directory.Exists(path);
            // フォルダを開く
            if (EditorGUIUtil.IconButton("d_FolderOpened Icon", "Open Explorer"))
            {
                OpenFolder(path);
            }
            // ターミナルを開く
            if (EditorGUIUtil.IconButton("d_BuildSettings.Standalone", "Open Terminal"))
            {
                OpenTerminal(path);
            }
            // リモートを開く
            GUI.enabled = old && !string.IsNullOrEmpty(RemoteUrl);
            {
                string remoteURLTooltip = string.Empty;
                if (!string.IsNullOrEmpty(RemoteUrl))
                {
                    remoteURLTooltip = $"Open Remote URL\n<color=grey>{RemoteUrl}</color>";
                }
                if (EditorGUIUtil.IconButton("d_Profiler.GlobalIllumination", remoteURLTooltip))
                {
                    OpenUrl(RemoteUrl);
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
            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = terminalPath,
                WorkingDirectory = path
            };
            if (!File.Exists(terminalPath))
            {
#if UNITY_EDITOR_WIN
                processInfo.FileName = "cmd";
#elif UNITY_EDITOR_OSX
                processInfo.FileName = "open";
                processInfo.Arguments = "-a Terminal";
#elif UNITY_EDITOR_LINUX
                processInfo.FileName = "gnome-terminal";
#endif
            }
            try
            {
                using Process process = Process.Start(processInfo);
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e.Message);
            }
        }
        void OpenUrl(string url) => OpenFolder(url);
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
