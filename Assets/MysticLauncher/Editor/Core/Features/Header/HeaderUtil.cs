using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class HeaderUtil
    {
        /// <summary>
        /// ミュート
        /// </summary>
        public static void MuteButton()
        {
            // トグルのスタイル
            GUIStyle toggleStyle = new GUIStyle(EditorStyles.toolbarButton);
            GUIContent content = EditorGUIUtil.NewIconContent(EditorUtility.audioMasterMute ? "d_SceneViewAudio@2x" : "d_SceneViewAudio On@2x");
            EditorUtility.audioMasterMute = GUILayout.Toggle(EditorUtility.audioMasterMute, content, toggleStyle, GUILayout.Width(25));
        }
        /// <summary>
        /// TimeScale
        /// </summary>
        public static void TimeScaleButton()
        {
            // トグルのスタイル
            GUIStyle toggleStyle = new GUIStyle(EditorStyles.toolbarButton);
            GUIContent content = EditorGUIUtil.NewIconContent("d_unityeditor.animationwindow", tooltip: "TimeScale");

            var rect = GUILayoutUtility.GetRect(25, EditorGUIUtility.singleLineHeight, toggleStyle);
            if (GUI.Button(rect, content, toggleStyle))
            {
                PopupWindow.Show(rect, new TimeScalePopup());
            }
        }
    }
}
