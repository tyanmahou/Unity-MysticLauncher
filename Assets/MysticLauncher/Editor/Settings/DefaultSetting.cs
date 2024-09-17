using System;
using System.Collections.Generic;
using UnityEditor;
using static Mystic.TreeElement;

namespace Mystic
{
    internal static class DefaultSetting
    {
        public static IElement[] CreatePortal()
        {
            List<IElement> portal = new List<IElement>();
            {
                var hello = CategoryElement.Create("Hello! Mystic Launcher!", icon: "Info@2x");
                hello.Elements = new IElement[]
                {
                    SettingServiceElement.CreateProject("Edit Project Portal", "Project/Mystic Launcher"),
                    RepositoryElement.Create("Project", "./")
                };
                portal.Add(hello);
            }
            {
                var tools = CategoryElement.Create("Tools", icon: "d_CustomTool@2x");
                tools.Elements = new IElement[]
                {
                    new ToolNaviElement()
                    {
                        Elements = new ActionElement[]
                        {
                            ActionElement.CreateMenuItem("Package", "d_Package Manager@2x", "Window/Package Manager"),
                            ActionElement.CreateMenuItem("Timeline", "UnityEditor.Timeline.TimelineWindow@2x", "Window/Sequencing/Timeline"),
                            ActionElement.CreateMenuItem("Test Runner", "Progress@2x", "Window/General/Test Runner"),
                            ActionElement.CreateMenuItem("Profiler", "d_UnityEditor.ProfilerWindow@2x", "Window/Analysis/Profiler %7"),
                            ActionElement.CreateMenuItem("Search","Search On Icon", "Window/Search/New Window"),
                        }
                    }
                };
                portal.Add(tools);
            }
            {
                var links = CategoryElement.Create("Links", icon: "d_Linked@2x");
                links.Elements = new IElement[]
                {
                    new TreeElement()
                    {
                        Elements = new Elem[]
                        {
                            new Elem()
                            {
                                Group = "Unity",
                                Element = URLElement.Create("Unity", "https://unity.com/")
                            },
                            new Elem()
                            {
                                Group = "Unity",
                                Element = URLElement.Create("Unity Document", "https://docs.unity3d.com/")
                            },
                            new Elem()
                            {
                                Group = "Unity",
                                Element = URLElement.Create("Unity Forum", "https://discussions.unity.com")
                            },
                            new Elem()
                            {
                                Group = "Mystic",
                                Element = URLElement.Create("Unity-MysticLauncher", "https://github.com/tyanmahou/Unity-MysticLauncher")
                            },
                        },
                    }
                };
                portal.Add(links);
            }
            return portal.ToArray();
        }
    }
}
