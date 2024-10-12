using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class RSSFeedLayout : ITabLayout
    {
        [SerializeField]
        private string[] _urls = new string[0];

        public string Title => "RSS";
        public Icon Icon
        {
            get
            {
                if (!_icon.IsValid)
                {
                    _icon = Icon.Create(MysticResource.LoadPackageAsset<Texture>("Icon/rss16.png"));
                }
                return _icon;
            }
        }

        public RSSFeedLayout() { }
        public RSSFeedLayout(IEnumerable<string> urls)
        {
            _urls = urls.ToArray();
        }

        public void OnGUI()
        {
            GUILayout.Space(5);
            using (new EditorGUILayout.HorizontalScope())
            {
                _searchString = EditorGUIUtil.ToolbarSearchField(_searchString);

                using (new EditorGUI.DisabledScope(_urls.Length <= 0))
                {
                    if (EditorGUIUtil.IconButton("d_Refresh", "Reload RSSFeed from URLs") || NeedReload())
                    {
                        FetchRSS();
                    }
                }
            }
            GUILayout.Space(5);
            EditorGUIUtil.DrawSeparator();
            if (_urls.Length <= 0)
            {
                EditorGUILayout.HelpBox("Please add RssFeed url. and Reload.", MessageType.Info);
                return;
            }

            LauncherWindow.Instance.Repaint();

            using(var scroller = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroller.scrollPosition;

                SetupStyle();
                int index = 0;
                foreach (var entry in _entries.Where(lsSearched))
                {
                    _boxStyle.normal.background = EditorGUIUtil.ListBackGroundTexture(index);
                    DrawEntry(entry);

                    ++index;
                }
            }
        }
        public override string ToString()
        {
            return Title;
        }
        private bool lsSearched(RSSFeedEntry entry)
        {
            if (entry.Channel.IsSearched(_searchString))
            {
                return true;
            }
            if (entry.Title.IsSearched(_searchString))
            {
                return true;
            }
            return false;
        }
        private void DrawEntry(RSSFeedEntry entry)
        {
            using (new GUILayout.VerticalScope(_boxStyle))
            {
                // 日付
                using (new GUILayout.HorizontalScope())
                {
                    EditorGUILayout.LabelField(entry.PublishDate.ToString("yyyy/MM/dd HH:mm:ss"), _dateStyle, GUILayout.MinWidth(0));
                    GUILayout.FlexibleSpace();
                    string tooltip = $"{entry.Channel}\n<color=grey>{entry.ChannelLink}</color>";
                    if (GUILayout.Button(new GUIContent(entry.Channel, tooltip), _channelStyle, GUILayout.MinWidth(0)))
                    {
                        Application.OpenURL(entry.ChannelLink);
                    }
                }
                {
                    string tooltip = $"{entry.Title}\n- {entry.Channel}\n<color=grey>{entry.Link}</color>";
                    var content = new GUIContent(entry.Title, tooltip);
                    if (GUILayout.Button(content, _titleStyle, GUILayout.MinWidth(0)))
                    {
                        Application.OpenURL(entry.Link);
                    }
                }
            }
        }
        private void FetchRSS()
        {
            _entries = RSSFeed.Fetch(_urls);
            _reloadTime = DateTime.Now;
        }
        bool NeedReload()
        {
            if (_urls.Length <= 0)
            {
                return false;
            }
            if (_entries is null)
            {
                return true;
            }
            return _reloadTime + TimeSpan.FromHours(1) < DateTime.Now;
        }
        void SetupStyle()
        {
            if (_boxStyle == null)
            {
                _boxStyle = new GUIStyle(EditorStyles.textField);
                _boxStyle.margin = new();
            }

            if (_dateStyle == null)
            {
                _dateStyle = new GUIStyle(EditorStyles.label);
                _dateStyle.fontSize = 11;
            }

            if (_channelStyle == null)
            {
                _channelStyle = new GUIStyle(GUI.skin.horizontalScrollbarThumb);
                _channelStyle.fontSize = 11;
                _channelStyle.margin = _dateStyle.margin;
                _channelStyle.margin.top += 3;
                var color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                _channelStyle.normal.textColor = color;
                _channelStyle.hover.textColor = color;
                _channelStyle.focused.textColor = color;
                _channelStyle.active.textColor = color;
                _channelStyle.fixedHeight = 15;
            }
            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(EditorStyles.linkLabel);
                _titleStyle.normal.textColor = new Color(0.506f, 0.706f, 1.0f, 1.0f);
                _titleStyle.hover.textColor = new Color(0.0f, 0.7f, 1.0f, 1.0f);
                _titleStyle.focused.textColor = new Color(0.0f, 0.7f, 1.0f, 1.0f);
                _titleStyle.active.textColor = new Color(0.0f, 0.7f, 1.0f, 1.0f);
            }
        }
        Icon _icon;
        List<RSSFeedEntry> _entries;

        string _searchString = string.Empty;
        Vector2 _scrollPosition;
        DateTime _reloadTime;

        static GUIStyle _boxStyle;
        static GUIStyle _dateStyle;
        static GUIStyle _channelStyle;
        static GUIStyle _titleStyle;
    }
}
