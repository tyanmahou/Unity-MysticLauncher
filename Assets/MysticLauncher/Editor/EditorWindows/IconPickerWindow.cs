using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Mystic
{
    public class IconPickerWindow : EditorWindow
    {
        private const int IconSize = 32;  // アイコンのサイズ

        public static void Show(SerializedProperty property)
        {
            var tex = property.FindPropertyRelative("_icon");
            var unityIcon = property.FindPropertyRelative("_unityIcon");
            var emoji = property.FindPropertyRelative("_emoji");
            Show(unityIcon, emoji, tex);
        }
        public static void Show(SerializedProperty iconProp, SerializedProperty emojiProp, SerializedProperty textureProp)
        {
            IconPickerWindow window = CreateInstance<IconPickerWindow>();
            window.titleContent = new GUIContent("Icon Picker");
            window.Init(iconProp, emojiProp, textureProp);
            window.ShowAuxWindow();
        }
        public static void Show(Action<Icon> callback, Icon icon = default)
        {
            IconPickerWindow window = CreateInstance<IconPickerWindow>();
            window.titleContent = new GUIContent("Icon Picker");
            window.Init(callback, icon);
            window.ShowAuxWindow();
        }
        void Init(SerializedProperty iconProp, SerializedProperty emojiProp, SerializedProperty textureProp)
        {
            Init(new SerializedReceiver(iconProp, emojiProp, textureProp));
        }
        void Init(Action<Icon> callback, Icon icon)
        {
            Init(new CallbackReceiver(callback, icon));
        }
        void Init(IIconReceiver receiver)
        {
            _iconReceiver = receiver;

            // アイコン名のリストを取得
            _iconNames = UnityIconUtil.GetIconNames();
            _normalStyle = new GUIStyle(GUI.skin.button);

            if (_iconReceiver.HasUnityIcon)
            {
                _selectedTab = 0;
            }
            if (_iconReceiver.HasEmoji)
            {
                _selectedTab = 1;
            }
            _isSkip = false;
        }
        private void OnDestroy()
        {
        }
        void OnGUI()
        {
            List<string> tabList = new(2)
            {
            };
            // Icon設定
            if (_iconReceiver.EditableUnityIcon)
            {
                tabList.Add("Icon");
            }
            // Emoji設定
            if (_iconReceiver.EditableEmoji)
            {
                tabList.Add("Emoji");
            }
            _selectedTab = GUILayout.Toolbar(
                _selectedTab,
                tabList.ToArray(),
                EditorStyles.toolbarButton,
                GUI.ToolbarButtonSize.FitToContents
                );

            // 検索バーの描画
            {
                _searchString = _searchField.OnGUI(_searchString);
            }

            // スクロールビュー開始
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            if (_selectedTab == 0)
            {
                DrawBuildIn();
            }
            else
            {
                DrawEmoji();
            }

            EditorGUILayout.EndScrollView();
            EditorGUIUtil.DrawSeparator();
            DrawFooter();
        }
        void DrawBuildIn()
        {
            // アイコン一覧の取得
            string[] icons = GetFilteredIcons(_searchString);
            GUIContent[] contents = icons.Select(EditorGUIUtility.IconContent).ToArray();

            void OnClick(int index) => SetIcon(icons[index]);
            void OnContext(int index) => ShowContextIcon(icons[index]);
            bool Selected(int index) => string.Equals(icons[index], _iconReceiver.UnityIcon, StringComparison.OrdinalIgnoreCase);

            DrawTextures(contents, OnClick, OnContext, Selected);
        }
        void DrawEmoji()
        {
            // アイコン一覧の取得
            string[] unicodes = EmojiUtil.GetUnicodeKeys(_searchString);
            GUIContent[] contents = unicodes.Select(u => new GUIContent((EmojiUtil.FromUnicodeKey(u)))).ToArray();
            void OnClick(int index) => SetEmoji(unicodes[index]);
            void OnContext(int index) => ShowContextEmoji(unicodes[index]);
            bool Selected(int index) => unicodes[index] == _iconReceiver.Emoji;

            DrawTextures(contents, OnClick, OnContext, Selected);
        }
        void DrawTextures(IReadOnlyList<GUIContent> iconContents, System.Action<int> onLeftClick, System.Action<int> onRightClick, System.Func<int, bool> selected)
        {
            int count = iconContents.Count;

            float w = this.position.width;
            int columns = Mathf.FloorToInt(w / IconSize);
            int rows = System.Math.Max(1, Mathf.CeilToInt(count / (float)columns));

            for (int row = 0; row < rows; row++)
            {
                using var horizontal = new EditorGUILayout.HorizontalScope();
                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;
                    if (index > count)
                        break;
                    if (index == 0)
                    {
                        Rect iconRect = GUILayoutUtility.GetRect(IconSize, IconSize, GUILayout.Width(IconSize), GUILayout.Height(IconSize));

                        if (GUI.Button(iconRect, "", _normalStyle))
                        {
                            if (Event.current.button == 0)
                            {
                                ResetProp();
                                if (_doubleClick.DoubleClick())
                                {
                                    Close();
                                }
                            }
                        }
                        if (IsEmptyProp())
                        {
                            EditorGUI.DrawRect(iconRect, _selectedColor);
                        }
                        continue;
                    }
                    --index;

                    Rect iconRect2 = GUILayoutUtility.GetRect(IconSize, IconSize, GUILayout.Width(IconSize), GUILayout.Height(IconSize));
                    if (GUI.Button(iconRect2, iconContents[index], _normalStyle))
                    {
                        if (Event.current.button == 0)
                        {
                            onLeftClick?.Invoke(index);
                            if (_doubleClick.DoubleClick())
                            {
                                Close();
                            }
                        }
                        else
                        {
                            onRightClick?.Invoke(index);
                        }
                    }
                    if (selected?.Invoke(index) ?? false)
                    {
                        EditorGUI.DrawRect(iconRect2, _selectedColor);
                        if (!_isSkip && Event.current.type == EventType.Repaint)
                        {
                            _isSkip = true;
                            _scrollPosition.y = iconRect2.y - IconSize * 3;
                        }
                    }
                }
            }
        }
        bool IsEmptyProp()
        {
            if (_iconReceiver.HasUnityIcon)
            {
                return false;
            }
            if (_iconReceiver.HasEmoji)
            {
                return false;
            }
            if (_iconReceiver.HasTexture)
            {
                return false;
            }
            return true;
        }
        void ResetProp() => SetProp(string.Empty, string.Empty, null);
        void SetIcon(string icon) => SetProp(icon, string.Empty, null);
        void SetEmoji(string name) => SetProp(string.Empty, name, null);
        void SetTexture(Texture texture) => SetProp(string.Empty, string.Empty, texture);
        void SetProp(string icon, string emoji, Texture tex) => _iconReceiver.Set(icon, emoji, tex);
        void DrawFooter()
        {
            // Texture設定
            if (_iconReceiver.EditableTexture)
            {
                var nextObj = EditorGUILayout.ObjectField(_iconReceiver.Texture, typeof(Texture), false);
                if (nextObj != _iconReceiver.Texture)
                {
                    SetTexture(nextObj as Texture);
                }
            }

            if (_iconReceiver.HasTexture)
            {
                var tex = _iconReceiver.Texture;
                GUIContent iconContent = new GUIContent(tex);
                iconContent.text = tex.name;
                GUILayout.Label(iconContent, GUILayout.Height(IconSize));
            }
            else if (_iconReceiver.HasUnityIcon)
            {
                GUIContent iconContent = new GUIContent(EditorGUIUtility.IconContent(_iconReceiver.UnityIcon));
                iconContent.text = _iconReceiver.UnityIcon;
                GUILayout.Label(iconContent, GUILayout.Height(IconSize));
            }
            else if (_iconReceiver.HasEmoji)
            {
                GUIContent iconContent = new GUIContent(EmojiUtil.FromUnicodeKey(_iconReceiver.Emoji));
                iconContent.text = EmojiUtil.GetShortName(_iconReceiver.Emoji) + $" <color=grey>({_iconReceiver.Emoji})</color>";
                EditorGUIUtil.RichLabel(iconContent, GUILayout.Height(IconSize));
            }
            else
            {
                GUILayout.Label("None", GUILayout.Height(IconSize));
            }
        }
        private void ShowContextIcon(string iconName)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy Icon Name"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = iconName;
            });

            menu.ShowAsContext();
        }
        private void ShowContextEmoji(string unicodeKey)
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = EmojiUtil.GetRaw(unicodeKey);
            });
            menu.AddItem(new GUIContent("Copy Unicode"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = unicodeKey;
            });
            menu.AddItem(new GUIContent("Copy ShortName"), false, () =>
            {
                EditorGUIUtility.systemCopyBuffer = EmojiUtil.GetShortName(unicodeKey);
            });
            menu.ShowAsContext();
        }
        private string[] GetFilteredIcons(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return _iconNames;
            }
            else
            {
                return _iconNames.Where(icon => icon.IsSearched(search)).ToArray();
            }
        }
        interface IIconReceiver
        {
            bool HasUnityIcon { get; }
            bool EditableUnityIcon { get; }
            string UnityIcon {get;}
            bool HasEmoji { get; }
            bool EditableEmoji { get; }
            string Emoji { get; }
            bool HasTexture { get; }
            bool EditableTexture { get; }
            Texture Texture { get; }

            void Set(string icon, string emoji, Texture tex);
        }
        class SerializedReceiver : IIconReceiver
        {
            public SerializedReceiver(SerializedProperty icon, SerializedProperty emoji, SerializedProperty texture)
            {
                _iconProp = icon;
                _emojiProp = emoji;
                _textureProp = texture;
            }
            public bool HasUnityIcon => _iconProp != null && !string.IsNullOrEmpty(_iconProp.stringValue);
            public bool EditableUnityIcon => _iconProp != null;
            public string UnityIcon => _iconProp.stringValue;
            public bool HasEmoji => _emojiProp != null && !string.IsNullOrEmpty(_emojiProp.stringValue);
            public bool EditableEmoji => _emojiProp != null;
            public string Emoji => _emojiProp.stringValue;
            public bool HasTexture => _textureProp != null &&
                _textureProp.propertyType == SerializedPropertyType.ObjectReference &&
                _textureProp.objectReferenceValue != null;
            public bool EditableTexture => _textureProp != null;
            public Texture Texture => _textureProp.objectReferenceValue as Texture;
            public void Set(string unityIcon, string emoji, Texture tex)
            {
                if (_iconProp != null)
                {
                    _iconProp.stringValue = unityIcon;
                    _iconProp.serializedObject.ApplyModifiedProperties();
                }
                if (_emojiProp != null)
                {
                    _emojiProp.stringValue = emoji;
                    _emojiProp.serializedObject.ApplyModifiedProperties();
                }
                if (_textureProp != null)
                {
                    _textureProp.objectReferenceValue = tex;
                    _textureProp.serializedObject.ApplyModifiedProperties();
                }
            }

            private SerializedProperty _iconProp;
            private SerializedProperty _emojiProp;
            private SerializedProperty _textureProp;
        }
        class CallbackReceiver : IIconReceiver
        {
            public CallbackReceiver(Action<Icon> callback, Icon init)
            {
                _callback = callback;
                _icon = init;
            }
            public bool HasUnityIcon => _icon.HasUnityIcon;
            public bool EditableUnityIcon => true;
            public string UnityIcon => _icon.UnityIcon;
            public bool HasEmoji => _icon.HasEmoji;
            public bool EditableEmoji => true;
            public string Emoji => _icon.Emoji;
            public bool HasTexture => _icon.HasTextureReference;
            public bool EditableTexture => true;
            public Texture Texture => _icon.TextureReference;
            public void Set(string unityIcon, string emoji, Texture tex)
            {
                if (tex != null)
                {
                    _icon = Icon.Create(tex);
                }
                else if (!string.IsNullOrEmpty(unityIcon))
                {
                    _icon = Icon.CreateUnityIcon(unityIcon);
                }
                else if (!string.IsNullOrEmpty(emoji))
                {
                    _icon = Icon.CreateEmoji(emoji);
                }
                else
                {
                    _icon = default;
                }
                _callback?.Invoke(_icon);
            }
            Action<Icon> _callback;
            Icon _icon;
        }
        IIconReceiver _iconReceiver;
        SearchField _searchField = new();
        private string _searchString = "";
        private Vector2 _scrollPosition;
        private string[] _iconNames;

        private DoubleClickCtrl _doubleClick = new();

        private GUIStyle _normalStyle;
        private Color _selectedColor = new Color(0, 1, 1, 0.3f);
        int _selectedTab;
        private bool _isSkip = false;
    }
}
