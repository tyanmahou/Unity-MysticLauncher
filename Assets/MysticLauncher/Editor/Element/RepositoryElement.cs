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
        public Label Label;

        [FolderSelect]
        public string LocalPath;

        public string RemoteUrl;

        public void OnGUI()
        {
            bool old = GUI.enabled;
            using var horizontal = new EditorGUILayout.HorizontalScope();
            var skin = new GUIStyle(EditorStyles.objectField);
            skin.richText = true;
            skin.margin.left += EditorGUI.indentLevel * 15;
            var label = $"{Label.Text} <color=grey>({LocalPath})</color>";
            if (Label.Icon.TryGetGUIContent(out var content))
            {
                content.text = label;
                content.tooltip = Label.Tooltip;
                GUILayout.Label(content, skin, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            else
            {
                GUILayout.Label(new GUIContent(label, Label.Tooltip), skin, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            var path = PathUtil.FixedPath(LocalPath);
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
            var terminalPath = LauncherUserSettings.instance.TerminalPath;
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
