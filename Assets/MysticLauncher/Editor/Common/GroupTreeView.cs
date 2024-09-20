using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// グループツリー表示
    /// </summary>
    /// <typeparam name="Elm"></typeparam>
    public class GroupTreeView<Elm>
    {
        public Func<Elm, string> GroupSelector { get; set; }
        public Action<Elm> DrawElementCallback {  get; set; }
        public Func<string, bool> DefaultToggleCallback { get; set; }

        public void OnGUI(IEnumerable<Elm> elements)
        {
            var root = MakeTree(elements);
            Draw(root);
        }
        void Draw(Node node)
        {
            foreach (Node child in node.Children.Values.OrderBy(n => n.Group))
            {
                if (!_toggle.TryGetValue(child.Group, out bool toggleOn))
                {
                    toggleOn = DefaultToggleCallback?.Invoke(child.Group) ?? true;
                    _toggle.Add(child.Group, toggleOn);
                }
                GUIContent contnet = EditorGUIUtil.FolderTogleContent(toggleOn, child.Name);
                toggleOn = EditorGUILayout.Foldout(toggleOn, contnet, true);
                _toggle[child.Group] = toggleOn;
                if (toggleOn)
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    Draw(child);
                }
            }
            using (new EditorGUI.IndentLevelScope())
            {
                foreach (var entry in node.Entries)
                {
                    DrawElementCallback?.Invoke(entry);
                }
            }
        }
        string GroupName(Elm entry)
        {
            return GroupSelector?.Invoke(entry) ?? entry.ToString();
        }
        Node MakeTree(IEnumerable<Elm> elements)
        {
            void Add(Node current, Elm entry, Span<string> splitGroup, string group)
            {
                if (splitGroup.Length <= 0)
                {
                    current.Entries.Add(entry);
                    return;
                }
                string name = splitGroup[0];
                if (string.IsNullOrEmpty(name))
                {
                    current.Entries.Add(entry);
                    return;
                }
                string nextGroup = group;
                if (string.IsNullOrEmpty(group))
                {
                    nextGroup += name;
                }
                else
                {
                    nextGroup += "/" + name;
                }
                if (!current.Children.TryGetValue(name, out Node child))
                {
                    child = new Node(name, nextGroup);
                    current.Children.Add(name, child);
                }
                Add(child, entry, splitGroup[1..], nextGroup);
            }
            Node root = new Node(string.Empty, string.Empty);
            foreach (Elm entry in elements)
            {
                Add(root, entry, GroupName(entry).Split('/'), string.Empty);
            }
            return root;
        }
        class Node
        {
            public Node(string name, string group)
            {
                Name = name;
                Group = group;
            }

            public string Name { get; }
            public string Group { get; }
            public Dictionary<string, Node> Children { get; } = new();
            public List<Elm> Entries { get; } = new();
        }
        Dictionary<string, bool> _toggle = new();
    }
}
