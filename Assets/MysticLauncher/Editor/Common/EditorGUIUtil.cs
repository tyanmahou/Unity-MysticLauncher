using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// GUIユーティリティ
    /// </summary>
    public static class EditorGUIUtil
    {
        public static GUIContent FolderTogleContent(bool open, string text)
        {
            GUIContent content = open ? FolderOpenedContent : FolderContent;
            content.text = text;
            return content;
        }
        static GUIContent FolderContent
        {
            get
            {
                if (_folderContent == null)
                {
                    _folderContent = new GUIContent(EditorGUIUtility.IconContent("d_Folder Icon"));
                }
                return _folderContent;
            }
        }
        static GUIContent FolderOpenedContent
        {
            get
            {
                if (_folderOpendContent == null)
                {
                    _folderOpendContent = new GUIContent(EditorGUIUtility.IconContent("d_FolderOpened Icon"));
                }
                return _folderOpendContent;
            }
        }
        static GUIContent _folderContent;
        static GUIContent _folderOpendContent;
        public static bool Button(in Label label)
        {
            return Button(label.Icon, label.Text ?? string.Empty, label.Tooltip ?? string.Empty, GUILayout.MinWidth(0), GUILayout.Height(EditorGUIUtility.singleLineHeight + 4));
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
        public static bool ButtonSquare(in Label label, float size = 60)
        {
            return ButtonSquare(label.Icon, label.Text ?? string.Empty, label.Tooltip ?? string.Empty, size);
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

        public static GUIContent NewIconContent(string iconName, string text = "", string tooltip = "")
        {
            var content = new GUIContent(EditorGUIUtility.IconContent(iconName));
            content.text = text;
            content.tooltip = tooltip;
            return content;
        }
        public static bool IconButton(string iconName, string tooltip = "")
        {
            var content = NewIconContent(iconName, string.Empty, tooltip);
            return GUILayout.Button(content, GUILayout.Width(30), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        }
        public static bool IconTextButton(string iconName, string text, string tooltip = "")
        {
            var content = NewIconContent(iconName, text, tooltip);
            return GUILayout.Button(content, GUILayout.Height(EditorGUIUtility.singleLineHeight + 4));
        }
        public static Texture2D ResizeTexture(Texture texture, int width, int height)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.sRGB
                );

            Graphics.Blit(texture, renderTex);
            RenderTexture.active = renderTex;
            Texture2D tempTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
            tempTexture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            tempTexture.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTex);

            // 16x16にリサイズ
            Texture2D resizedTexture = new Texture2D(width, height);

            // 縮小処理
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // リサンプリング: 元のテクスチャのピクセル位置を計算
                    float u = x / (float)width;
                    float v = y / (float)height;

                    // 元のテクスチャから対応するピクセルの色を取得
                    Color color = tempTexture.GetPixelBilinear(u, v);

                    // 新しいテクスチャにピクセルをセット
                    resizedTexture.SetPixel(x, y, color);
                }
            }
            Object.DestroyImmediate(tempTexture);
            resizedTexture.Apply();

            return resizedTexture;
        }
    }
}
