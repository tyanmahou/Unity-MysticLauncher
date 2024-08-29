using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class ActionElement : IElement
    {
        public Label Label;

        [SerializeReference, SubclassSelector]
        private IToolAction _action;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
            {
                _action?.Execute();
            }
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
