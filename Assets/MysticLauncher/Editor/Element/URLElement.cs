using System;

namespace Mystic
{
    [Serializable]
    public class URLElement : ActionElement<OpenUrlAction>
    {
        public static URLElement Create(string text, string url, string tooltip = "", string icon = null)
        {
            return new URLElement()
            {
                Label = Label.Create(text, tooltip, icon),
                _action = new OpenUrlAction()
                {
                    URL = url,
                }
            };
        }
        protected override string DefaultTooltip()
        {
            if (_action is null)
            {
                return string.Empty;
            }
            if (string.IsNullOrEmpty(_action.URL))
            {
                return string.Empty;
            }
            return $"Open URL\n<color=grey>{_action.URL}</color>";
        }
    }
}
