using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Mystic
{
    /// <summary>
    /// RSS Feed の取得
    /// </summary>
    public static class RSSFeed
    {
        public static List<RSSFeedEntry> Fetch(params string[] urls)
        {
            List<RSSFeedEntry> result = new List<RSSFeedEntry>();
            foreach (string s in urls)
            {
                try
                {
                    result.AddRange(Fetch(s));
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogWarning($"[RSSFeed] Fetch error ({s})\n{ex.Message}");
                }
            }
            result = result.OrderByDescending(x => x.PublishDate).ToList();
            return result;
        }
        static IEnumerable<RSSFeedEntry> Fetch(string url)
        {
            // RSS読み込み
            XElement rssXmlDoc = XElement.Load(url);
            string rootName = rssXmlDoc.Name.LocalName;
            if (rootName == "rss")
            {
                // RSS 2.0 フィードの処理
                string channel = (string)rssXmlDoc.Element("channel")?.Element("title")?.Value;
                string channelLink = (string)rssXmlDoc.Element("channel")?.Element("link")?.Value;
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
                    yield return rssEntry;
                }
            }
            else if (rootName == "feed")
            {
                // Atom フィードの処理
                string ns = "http://www.w3.org/2005/Atom";
                string channel = (string)rssXmlDoc.Element(XName.Get("title", ns));
                string channellLink = GetChannelLink(rssXmlDoc, ns);
                foreach (var entry in rssXmlDoc.Descendants(XName.Get("entry", ns)))
                {
                    var feedEntry = new RSSFeedEntry()
                    {
                        Channel = channel,
                        ChannelLink = channellLink,
                        Title = (string)entry.Element(XName.Get("title", ns)),
                        Link = (string)entry.Element(XName.Get("link", "http://www.w3.org/2005/Atom"))?.Attribute("href"),
                        PublishDate = DateTime.Parse((string)entry.Element(XName.Get("published", ns))),
                    };
                    yield return feedEntry;
                }
            }
        }
        static string GetChannelLink(XElement rssXmlDoc, string ns)
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
    }
}
