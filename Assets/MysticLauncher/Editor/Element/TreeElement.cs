using System;
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
            _tree.GroupSelector = elem => elem.Group;
            _tree.DrawElementCallback = elem =>
            {
                elem?.Element?.OnGUI();
            };
            _tree.OnGUI(Elements);
        }

        public override string ToString()
        {
            return "Tree";
        }

        GroupTreeView<Elem> _tree = new();
    }
}
