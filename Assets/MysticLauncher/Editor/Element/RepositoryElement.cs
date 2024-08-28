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
                GUILayout.Label(content, skin, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            else
            {
                GUILayout.Label(label, skin, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            var path = PathUtil.RelativeOrFullPath(LocalPath);
            GUI.enabled = old && System.IO.Directory.Exists(path);
            // フォルダを開く
            {
                GUIContent icon = EditorGUIUtility.IconContent("d_FolderOpened Icon");
                if (GUILayout.Button(icon, GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    OpenFolder(path);
                }
            }
            // ターミナルを開く
            {
                GUIContent icon = EditorGUIUtility.IconContent("d_BuildSettings.Standalone");
                if (GUILayout.Button(icon, GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    OpenTerminal(path);
                }
            }
            // リモートを開く
            GUI.enabled = old && !string.IsNullOrEmpty(RemoteUrl);
            {
                GUIContent icon = EditorGUIUtility.IconContent("d_Profiler.GlobalIllumination");
                if (GUILayout.Button(icon, GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
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
        void OpenUrl(string url) => OpenFolder(url);
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
