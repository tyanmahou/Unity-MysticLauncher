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
        public delegate bool ToggleFunc(Node node);

        public class Node
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
        

        public Func<Elm, string> GroupSelector { get; set; }
        public Action<Elm> DrawElementCallback {  get; set; }
        public ToggleFunc DefaultToggleCallback { get; set; }
        public ToggleFunc ForceOpenToggle { get; set; }
        public Action<Node, Action<Node>> DrawGroupDecorater{ get; set; }

        public void OnGUI(IEnumerable<Elm> elements)
        {
            _root = MakeTree(elements);

            if (DrawGroupDecorater is null)
            {
                Draw(_root);
            }
            else
            {
                DrawGroupDecorater?.Invoke(_root, Draw);
            }
        }

        public void Toggle(string group, bool on)
        {
            if (!_toggle.TryGetValue(group, out var toggleAnim))
            {
                _toggle.Add(group, new ToggleAnimBool(on));
            }
            else
            {
                toggleAnim.IsOn = on;
            }
        }
        public void Toggle(bool on) => Toggle(_root, on);
        public void ToggleOn() => Toggle(true);
        public void ToggleOff() => Toggle(false);
        void Toggle(Node node, bool on)
        {
            if (node is null)
            {
                return;
            }
            foreach (Node child in node.Children.Values)
            {
                Toggle(child.Group, on);
                Toggle(child, on);
            }
        }

        void Draw(Node node)
        {
            void DrawChild(Node child)
            {
                if (!_toggle.TryGetValue(child.Group, out var toggleAnim))
                {
                    toggleAnim = new(DefaultToggleCallback?.Invoke(child) ?? true);
                    _toggle.Add(child.Group, toggleAnim);
                }
                bool prevToggle = toggleAnim.IsOn;
                GUIContent contnet = EditorGUIUtil.FolderTogleContent(prevToggle, child.Name);
                bool nextToggle = EditorGUILayout.Foldout(prevToggle, contnet, true);
                if (ForceOpenToggle != null && ForceOpenToggle(child))
                {
                    nextToggle = true;
                }
                toggleAnim.IsOn = nextToggle;
                if (EditorGUILayout.BeginFadeGroup(toggleAnim.Faded))
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        Draw(child);
                    }
                }
                EditorGUILayout.EndFadeGroup();
            }

            foreach (Node child in node.Children.Values)
            {

                if (DrawGroupDecorater is null)
                {
                    DrawChild(child);
                }
                else
                {
                    DrawGroupDecorater?.Invoke(child, DrawChild);
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
        Dictionary<string, ToggleAnimBool> _toggle = new();
        Node _root;
    }
}
