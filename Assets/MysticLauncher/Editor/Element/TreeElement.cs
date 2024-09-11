using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class TreeElement : IElement
    {
        [Serializable]
        public class Elem
        {
            public string Group;
            [SerializeReference, SubclassSelector]
            public IElement Element;

            public override string ToString()
            {
                var path = Group;
                if (Element != null)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        path += Element.ToString();
                    }
                    else
                    {
                        path += "/" + Element.ToString();
                    }
                }
                return path;
            }
        }

        [NamedArrayElement]
        public Elem[] Elements = new Elem[0];

        public void OnGUI()
        {
            Dictionary<string, List<Elem>> dic = new();
            foreach (var entry in Elements)
            {
                if (!dic.TryGetValue(entry.Group, out var list))
                {
                    list = new List<Elem>();
                    dic.Add(entry.Group, list);
                }
                list.Add(entry);
            }
            Draw(Elements, dic, string.Empty);
        }

        public override string ToString()
        {
            return "Tree";
        }

        void Draw(IEnumerable<Elem> elmList, Dictionary<string, List<Elem>> dic, string path)
        {
            elmList = GetNextPaths(elmList, path);
            foreach (string next in CalcNextPathNames(elmList, path))
            {
                string nextFullPath = path;
                if (string.IsNullOrEmpty(path))
                {
                    nextFullPath = next;
                }
                else
                {
                    nextFullPath += "/" + next;
                }
                if (!_toggle.ContainsKey(nextFullPath))
                {
                    _toggle[nextFullPath] = true;
                }
                
                GUIContent folderContent = EditorGUIUtil.FolderTogleContent(_toggle[nextFullPath], next);
                _toggle[nextFullPath] = EditorGUILayout.Foldout(_toggle[nextFullPath], folderContent, true);

                if (_toggle[nextFullPath])
                {
                    using var indent = new EditorGUI.IndentLevelScope();
                    Draw(elmList, dic, nextFullPath);
                }
            }
            if (dic.TryGetValue(path, out var list))
            {
                foreach (var entry in list)
                {
                    entry.Element?.OnGUI();
                }
            }
        }

        IEnumerable<Elem> GetNextPaths(IEnumerable<Elem> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list.Where(f => f.Group.StartsWith(path) && f.Group != path);
        }
        IEnumerable<string> CalcNextPathNames(IEnumerable<Elem> list, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += "/";
            }
            return list
                .Select(f => f.Group)
                .Select(s => s.Substring(path.Length).Split('/')[0])
                .Distinct()
                ;
        }
        Dictionary<string, bool> _toggle = new();
    }
}
