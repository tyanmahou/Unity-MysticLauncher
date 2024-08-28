using System;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// GUIユーティリティ
    /// </summary>
    public static class EditorGUIUtil
    {
        public static bool Button(in Label label)
        {
            return Button(label.Icon, label.Text, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight + 2));
        }
        public static bool Button(Icon icon, string label, params GUILayoutOption[] options)
        {
            var skin = new GUIStyle(GUI.skin.button);
            skin.margin.left += EditorGUI.indentLevel * 15;
            skin.alignment = TextAnchor.MiddleLeft;
            if (icon.TryGetGUIContent(out var content))
            {
                content.text = label;
                return GUILayout.Button(content, skin, options);
            }
            return GUILayout.Button(label, skin, options);
        }
        public static void DrawSeparator()
        {
            var rect = GUILayoutUtility.GetRect(1f, 1f);
            rect.xMin = 0;
            rect.width += 4f;
            EditorGUI.DrawRect(rect, !EditorGUIUtility.isProSkin
                ? new Color(0.6f, 0.6f, 0.6f, 1.333f)
                : new Color(0.12f, 0.12f, 0.12f, 1.333f)
                );
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
