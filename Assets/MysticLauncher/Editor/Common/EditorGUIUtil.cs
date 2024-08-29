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
            return Button(label, string.Empty);
        }
        public static bool Button(in Label label, string tooltip)
        {
            return Button(label.Icon, label.Text, tooltip, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight + 2));
        }
        public static bool Button(Icon icon, string label, string tooltip, params GUILayoutOption[] options)
        {
            var skin = new GUIStyle(GUI.skin.button);
            skin.margin.left += EditorGUI.indentLevel * 15;
            skin.alignment = TextAnchor.MiddleLeft;
            if (icon.TryGetGUIContent(out var content))
            {
                content.text = label;
                content.tooltip = tooltip;
                return GUILayout.Button(content, skin, options);
            }
            return GUILayout.Button(new GUIContent(label, tooltip), skin, options);
        }
        public static bool ButtonSquare(in Label label, string tooltip, float size = 60)
        {
            return ButtonSquare(label.Icon, label.Text, tooltip, size);
        }
        public static bool ButtonSquare(Icon icon, string label, string tooltip, float size = 60)
        {

            // Button
            Rect buttonRect = GUILayoutUtility.GetRect(size, size, GUI.skin.button, GUILayout.MaxHeight(size));
            bool isClick = false;
            if (GUI.Button(buttonRect, new GUIContent(string.Empty, tooltip)))
            {
                isClick = true;
            }
            Rect iconRect = buttonRect;
            float iconSize = Mathf.Min(30, size - GUI.skin.button.margin.top * 2 - EditorGUIUtility.singleLineHeight);
            iconRect.x += (iconRect.width - iconSize) / 2.0f;
            iconRect.width = iconSize;
            if (string.IsNullOrEmpty(label))
            {
                iconRect.y += (iconRect.height - iconSize) / 2.0f;
            }
            else
            {
                iconRect.y += (iconRect.height - EditorGUIUtility.singleLineHeight - iconSize) / 2.0f;
            }
            iconRect.height = iconSize;
            float fixedSize = buttonRect.width - GUI.skin.button.margin.left * 2;
            if (icon.TryGetGUIContent(out var content)) {
                GUI.DrawTexture(iconRect, content.image, ScaleMode.ScaleToFit);

                if (!string.IsNullOrEmpty(label))
                {
                    Rect labelRect = buttonRect;
                    labelRect.y = iconRect.yMax - EditorGUIUtility.singleLineHeight - GUI.skin.button.margin.bottom;
                    GUIStyle labelStyle = GetFitStyle(label, fixedSize, buttonRect.yMax - iconRect.yMax);
                    labelStyle.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(labelRect, label, labelStyle);
                }
            }
            else if (!string.IsNullOrEmpty(label))
            {
                GUIStyle labelStyle = GetFitStyle(label, fixedSize, buttonRect.height - GUI.skin.button.margin.top * 2);
                labelStyle.alignment = TextAnchor.MiddleCenter;
                GUI.Label(buttonRect, label, labelStyle);
            }
            return isClick;
        }
        public static GUIStyle GetFitStyle(string text, float w, float h)
        {
            var content = new GUIContent(text);
            GUIStyle style = new GUIStyle(GUI.skin.label);
            GUIStyle style2 = new GUIStyle(GUI.skin.label);
            style2.fixedWidth = w;
            style2.wordWrap = true;
            while (style.fontSize > 1)
            {
                Vector2 textSize = style.CalcSize(content);
                if (textSize.x <= w)
                {
                    break;
                }
                float textSize2 = style2.CalcHeight(content, w);
                if (textSize2 <= h)
                {
                    break;
                }
                --style.fontSize;
                --style2.fontSize;
            }
            style.wordWrap = true;
            return style;
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
