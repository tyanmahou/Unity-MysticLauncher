using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class ActionElement : IElement
    {
        public Label Label;

        [SerializeReference, SubclassSelector]
        public IToolAction _action;

        public void OnGUI()
        {
            if (EditorGUIUtil.Button(Label))
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
    public abstract class ActionElement<T> : IElement
        where T : IToolAction
    {
        public Label Label;

        [SerializeField, Flatten]
        protected T _action;

        public void OnGUI()
        {
            var label = Label;
            if (string.IsNullOrEmpty(label.Tooltip))
            {
                label.Tooltip = DefaultTooltip();
            }
            if (EditorGUIUtil.Button(label))
            {
                Execute();
            }
        }
        public void Execute()
        {
            _action?.Execute();
        }
        protected virtual string DefaultTooltip()
        {
            return string.Empty;
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
