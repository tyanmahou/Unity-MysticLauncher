using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class ActionElement : IElement
    {
        public Label Label;
        public string Tooltip;

        [SerializeReference, SubclassSelector]
        public IToolAction _action;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label, Tooltip))
            {
                Execute();
            }
        }
        public void Execute()
        {
            _action?.Execute();
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
