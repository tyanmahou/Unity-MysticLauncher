using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    public class RSSFeedEntry
    {
        public string Channel;
        public string ChannelLink;
        public string Title;
        public string Link;
        public DateTime PublishDate;
    }
    [Serializable]
    public class RSSFeedLayout : ITabLayout
    {
        [SerializeField]
        private string[] _urls = new string[0];

        public string Title => "RSS";
        public Icon Icon => default;

        public RSSFeedLayout() { }
        public RSSFeedLayout(IEnumerable<string> urls)
        {
            _urls = urls.ToArray();
        }

        public void OnGUI()
        {
            LauncherWindow.Instance.Repaint();
            _searchString = EditorGUIUtil.ToolbarSearchField(_searchString);
            var reloadContent = EditorGUIUtil.NewIconContent("d_Refresh", "Reload", "Reload RSSFeed from URLs");
            if (GUILayout.Button(reloadContent) || NeedReload())
            {
                FetchRSS();
            }
            EditorGUIUtil.DrawSeparator();
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
            _entries = new List<RSSFeedEntry>();
            _reloadTime = DateTime.Now;

            foreach (var url in _urls)
            {
                FetchRSS(url);
            }
            _entries = _entries.OrderByDescending(x => x.PublishDate).ToList();
        }
        private void FetchRSS(string url)
        {
            try
            {
                // RSS読み込み
                XElement rssXmlDoc = XElement.Load(url);
                string rootName = rssXmlDoc.Name.LocalName;
                if (rootName == "rss")
                {
                    string channel = (string)rssXmlDoc.Element("channel")?.Element("title")?.Value;
                    string channelLink = (string)rssXmlDoc.Element("channel")?.Element("link")?.Value;
                    // RSS 2.0 フィードの処理
                    foreach (var item in rssXmlDoc.Descendants("item"))
                    {
                        var rssEntry = new RSSFeedEntry()
                        {
                            Channel = channel,
                            ChannelLink = channelLink,
                            Title = (string)item.Element("title"),
                            Link = (string)item.Element("link"),
                            PublishDate = DateTime.Parse((string)item.Element("pubDate")),
                        };
                        _entries.Add(rssEntry);
                    }
                }
                else if (rootName == "feed")
                {
                    string ns = "http://www.w3.org/2005/Atom";
                    string channel = (string)rssXmlDoc.Element(XName.Get("title", ns));
                    string channellLink = GetChannelLink(rssXmlDoc, ns);
                    // Atom フィードの処理
                    foreach (var entry in rssXmlDoc.Descendants(XName.Get("entry", ns)))
                    {
                        var feedEntry = new RSSFeedEntry()
                        {
                            Channel = channel,
                            ChannelLink= channellLink,
                            Title = (string)entry.Element(XName.Get("title", ns)),
                            Link = (string)entry.Element(XName.Get("link", "http://www.w3.org/2005/Atom"))?.Attribute("href"),
                            PublishDate = DateTime.Parse((string)entry.Element(XName.Get("published", ns))),
                        };
                        _entries.Add(feedEntry);
                    }
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Fetch RSSFeed error ({url})\n{e.Message}");
            }
        }
        public string GetChannelLink(XElement rssXmlDoc, string ns)
        {
            // <link>要素をすべて取得
            var links = rssXmlDoc.Descendants(XName.Get("link", ns));

            // 1. 要素の中身があるもの
            var linkWithValue = links.FirstOrDefault(link => !string.IsNullOrWhiteSpace(link.Value));

            // 2. rel="alternate"のものを取得
            var alternateLink = links.FirstOrDefault(link => (string)link.Attribute("rel") == "alternate");

            // 3. rel="self"のものを取得
            var selfLink = links.FirstOrDefault(link => (string)link.Attribute("rel") == "self");

            // 優先度に従ってリンクを選択
            if (linkWithValue != null)
            {
                return linkWithValue.Value; // 最優先: 値が設定されているリンク
            }
            else if (alternateLink != null)
            {
                return (string)alternateLink.Attribute("href"); // 次に優先: rel="alternate"
            }
            else if (selfLink != null)
            {
                return (string)selfLink.Attribute("href"); // 最後に優先: rel="self"
            }

            return (string)links.FirstOrDefault();
        }
        bool NeedReload()
        {
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
        private List<RSSFeedEntry> _entries;

        private string _searchString = string.Empty;
        private Vector2 _scrollPosition;

        private static GUIStyle _boxStyle;
        private static GUIStyle _dateStyle;
        private static GUIStyle _channelStyle;
        private static GUIStyle _titleStyle;
        private static DateTime _reloadTime;
    }
}
