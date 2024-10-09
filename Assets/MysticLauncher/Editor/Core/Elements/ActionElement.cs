using System;
using UnityEngine;

namespace Mystic
{
    [Serializable]
    public class ActionElement : IElement
    {
        public static ActionElement CreateMenuItem(string labelText, string icon, string menuItem)
        {
            return new ActionElement()
            {
                Label = Label.Create(labelText, icon: icon),
                Action = new MenuItemAction()
                {
                    ItemName = menuItem,
                }
            };
        }
        public Label Label;

        [SerializeField, SerializeReference, SubclassSelector]
        private IToolAction _action;

        public IToolAction Action
        {
            get => _action;
            set => _action = value;
        }

        public Label LabelOverridedTooltip
        {
            get
            {
                var label = Label;
                if (string.IsNullOrEmpty(label.Tooltip))
                {
                    label.Tooltip = Action?.Tooltip() ?? string.Empty;
                }
                return label;
            }
        }
        public void OnGUI()
        {
            if (EditorGUIUtil.Button(LabelOverridedTooltip))
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

        public T Action
        {
            get => _action;
            set => _action = value;
        }
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
            return _action?.Tooltip() ?? string.Empty;
        }
        public override string ToString()
        {
            return Label.Text;
        }
    }
}
