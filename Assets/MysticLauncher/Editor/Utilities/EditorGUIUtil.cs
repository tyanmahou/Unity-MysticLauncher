using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// GUIユーティリティ
    /// </summary>
    public static class EditorGUIUtil
    {
        static GUIStyle RichStyle
        {
            get
            {
                if (_richStyle == null)
                {
                    _richStyle = new GUIStyle(GUI.skin.label);
                    _richStyle.richText = true;
                }
                return _richStyle;
            }
        }
        static GUIStyle _richStyle;
        /// <summary>
        /// リッチテキスト許容のラベルフィールド
        /// </summary>
        /// <param name="content"></param>
        /// <param name="options"></param>
        public static void RichLabel(GUIContent content, params GUILayoutOption[] options)
        {
            GUILayout.Label(content, RichStyle, options);
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
        /// <summary>
        /// フォルダアイコンのトグル
        /// </summary>
        /// <param name="open"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static GUIContent FolderTogleContent(bool open, string text)
        {
            GUIContent content = open ? FolderOpenedContent : FolderContent;
            content.text = text;
            return content;
        }
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
            EditorGUI.DrawRect(rect, new Color(0.12f, 0.12f, 0.12f, 1.333f));
        }

        public static GUIContent NewIconContent(string iconName, string text = "", string tooltip = "")
        {
            var content = new GUIContent(EditorGUIUtility.IconContent(iconName));
            content.text = text;
            content.tooltip = tooltip;
            return content;
        }
        public static Rect GetFixRect(float width, float height, GUIStyle style)
        {
            return GUILayoutUtility.GetRect(width, height, style, GUILayout.Width(width), GUILayout.Height(height));
        }
        public static Rect GetFixRect(float width, float height)
        {
            return GetFixRect(width, height, GUIStyle.none);
        }
        public static bool IconButton(string iconName, string tooltip = "")
        {
            const float buttonWidth = 26f;
            var skin = new GUIStyle(GUI.skin.button);
            skin.margin.left = skin.margin.right = 1;
            var rect = GetFixRect(buttonWidth, EditorGUIUtility.singleLineHeight, skin);
            bool ret = GUI.Button(rect, new GUIContent(string.Empty, tooltip));
            var image = EditorGUIUtility.IconContent(iconName).image;

            const float size = 16;
            rect.x = rect.center.x - size / 2;
            rect.width = size;
            rect.y = rect.center.y - size / 2;
            rect.height = size;
            var old = GUI.color;
            GUI.color = GUI.enabled ? Color.white : new Color(1,1,1,0.5f);
            GUI.DrawTexture(rect, image, ScaleMode.ScaleToFit);
            GUI.color = old;
            return ret;
        }
        public static bool IconTextButton(string iconName, string text, string tooltip = "")
        {
            var content = NewIconContent(iconName, text, tooltip);
            return GUILayout.Button(content, GUILayout.Height(EditorGUIUtility.singleLineHeight + 4));
        }

        public static bool TruncateFit(GUIContent label, float width, GUIStyle style)
        {
            Vector2 labelSize = style.CalcSize(label);

            // テキストがボタンと重なる場合は、テキストを短縮
            if (labelSize.x > width)
            {
                string src = label.text;
                label.text += "...";
                while (style.CalcSize(label).x > width)
                {
                    if (src.Length > 1)
                    {
                        src = src.Substring(0, src.Length - 1);
                        label.text = src + "...";
                    }
                    else
                    {
                        break;
                    }
                }
                return true;
            }
            return false;
        }
        public static GUILayout.VerticalScope ScopedMargin(int left, int right, int top, int bottom)
        {
            var style = new GUIStyle();
            style.margin = new RectOffset()
            {
                left = left,
                right = right,
                top = top,
                bottom = bottom,
            };
            return new GUILayout.VerticalScope(style);
        }
        public static GUILayout.VerticalScope ScopedMargin(int leftRight, int topBottom)
        {
            return ScopedMargin(leftRight, leftRight, topBottom, topBottom);
        }
        public static GUILayout.VerticalScope ScopedMargin(int margin = 5)
        {
            return ScopedMargin(margin, margin);
        }
        public static int MultiLineToolBar(int selected, IEnumerable<GUIContent> contents)
        {
            return MultiLineToolBar(selected, contents, EditorStyles.toolbarButton);
        }
        public static int MultiLineToolBar(int selected, IEnumerable<GUIContent> contents, GUIStyle style)
        {
            float width = EditorGUIUtility.currentViewWidth;
            float height = style.fixedHeight;
            float totalW = 0;
            float totalH = height;
            foreach (var content in contents)
            {
                var size = style.CalcSize(content);
                totalW += size.x;
                if (totalW > width)
                {
                    totalW = 0;
                    totalH += height;
                }
            }
            var rect = GUILayoutUtility.GetRect(width, totalH);
            rect.height = height;
            totalW = 0;
            var c = rect;
            var x = rect.x;
            foreach (var (content, i) in contents.Select((content, index) => (content, index)))
            {
                var size = style.CalcSize(content);
                totalW += size.x;
                if (totalW > width)
                {
                    totalW = 0;
                    rect.x = x;
                    rect.y += height;
                }
                var cs = rect;
                cs.width = size.x;
                rect.x += size.x;
                if (GUI.Toggle(cs, i == selected, content, style))
                {
                    selected = i;
                }
            }
            return selected;
        }
        public static int ScrollToolBar(in Rect position, int selected, ref float scrollX, float deltaTime, IEnumerable<GUIContent> contents, System.Action<int> onContext = null)
        {
            GUIStyle style = EditorStyles.toolbarButton;
            float height = style.fixedHeight;
            float totalW = 0;
            foreach (var content in contents)
            {
                var size = style.CalcSize(content);
                totalW += size.x;
            }
            Rect scrollRect = position;
            if (onContext != null)
            {
                scrollRect.width -= 20;
            }
            scrollRect.height = height;
            var contentsWithIndex = contents.Select((content, index) => (content, index));
            using (new GUILayout.HorizontalScope())
            {
                using (var scrollView = new SimpleHorizontalScrollScope(scrollRect, scrollX, totalW, deltaTime))
                {
                    Rect rect = GUILayoutUtility.GetRect(totalW, height);
                    foreach (var (content, i) in contentsWithIndex)
                    {
                        var size = style.CalcSize(content);
                        rect.width = size.x;
                        if (GUI.Toggle(rect, i == selected, content, style))
                        {
                            selected = i;
                        }
                        rect.x += rect.width;
                    }
                    scrollX = scrollView.scrollX;
                }
                if (onContext != null)
                {
                    // Menu
                    var menuStyle = new GUIStyle(EditorStyles.iconButton);
                    menuStyle.alignment = TextAnchor.MiddleCenter;
                    menuStyle.fixedHeight = height;
                    if (GUILayout.Button(EditorGUIUtility.IconContent("_Menu"), menuStyle, GUILayout.Width(20)))
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.allowDuplicateNames = true;
                        foreach (var (content, i) in contentsWithIndex)
                        {
                            int copy = i;
                            menu.AddItem(content, i == selected, () =>
                            {
                                onContext?.Invoke(copy);
                            });
                        }
                        menu.ShowAsContext();
                    }
                }
            }
            if (selected >= contents.Count())
            {
                selected = 0;
            }
            return selected;
        }
        public static Rect GetNoVericalSpaceRect()
        {
            return GUILayoutUtility.GetRect(0, -EditorGUIUtility.standardVerticalSpacing);
        }
        public static float GetWidth()
        {
            return EditorGUIUtility.currentViewWidth;
        }
        public static Rect GetIndentedRect()
        {
            var rect = GetNoVericalSpaceRect();
            rect.x += EditorGUI.indentLevel * 15;
            rect.width -= EditorGUI.indentLevel * 15;
            return rect;
        }
        public static float GetIndentedWidth()
        {
            return GetWidth() - EditorGUI.indentLevel * 15;
        }
        public static GUIContent GetIconContent16x16(in Label label)
        {
            return GetIconContent16x16(label.Text, label.Tooltip, label.Icon);
        }
        public static GUIContent GetIconContent16x16(in Icon icon)
        {
            return GetIconContent16x16(string.Empty, string.Empty, icon);
        }
        public static GUIContent GetIconContent16x16(string text, in Icon icon)
        {
            return GetIconContent16x16(text, string.Empty, icon);
        }
        public static GUIContent GetIconContent16x16(string text, string tooltip, in Icon icon)
            => GetIconContent(_iconTextures16x16, text, tooltip, icon, 16, 16);
        public static GUIContent GetIconContent32x32(in Label label)
        {
            return GetIconContent32x32(label.Text, label.Tooltip, label.Icon);
        }
        public static GUIContent GetIconContent32x32(in Icon icon)
        {
            return GetIconContent32x32(string.Empty, string.Empty, icon);
        }
        public static GUIContent GetIconContent32x32(string text, in Icon icon)
        {
            return GetIconContent32x32(text, string.Empty, icon);
        }
        public static GUIContent GetIconContent32x32(string text, string tooltip, in Icon icon)
            => GetIconContent(_iconTextures32x32, text, tooltip, icon, 32, 32);
        static GUIContent GetIconContent(Dictionary<Texture, Texture> cache, string text, string tooltip, in Icon icon, int w, int h)
        {
            if (icon.TryGetGUIContent(out var content))
            {
                if (content.image != null)
                {
                    if (!cache.TryGetValue(content.image, out Texture tex))
                    {
                        TryResizeTexture(content.image, w, h, out tex);
                        cache.Add(content.image, tex);
                    }
                    if (tex == null)
                    {
                        TryResizeTexture(content.image, w, h, out tex);
                        cache[content.image] = tex;
                    }
                    content.image = tex;
                }
                content.text = text;
                content.tooltip = tooltip;
                return content;
            }
            return new GUIContent(text, tooltip);
        }
        public static bool TryResizeTexture(Texture texture, int width, int height, out Texture result)
        {
            if (texture.width == width && texture.height == height)
            {
                result = texture as Texture2D;
                return false;
            }
            result = ResizeTexture(texture, width, height);
            return true;
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
        public static Texture2D ListBackGroundTexture(int index)
        {
            if (index % 2 == 0)
            {
                return ColorTexture(new Color32(56, 56, 56, 255));
            }
            else
            {
                return ColorTexture(new Color32(63, 63, 63, 255));
            }
        }
        public static Texture2D ColorTexture(in Color color)
        {
            if (_colorTextures.TryGetValue(color, out Texture2D texture))
            {
                return texture;
            }
            texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            _colorTextures.TryAdd(color, texture);
            return texture;
        }
        static Dictionary<Texture, Texture> _iconTextures16x16 = new();
        static Dictionary<Texture, Texture> _iconTextures32x32 = new();
        static Dictionary<Color, Texture2D> _colorTextures = new();
    }
}
