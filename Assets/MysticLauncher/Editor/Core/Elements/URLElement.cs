using System;

namespace Mystic
{
    [Serializable]
    public class URLElement : ActionElement<OpenURLAction>
    {
        public static URLElement Create(string text, string url, string tooltip = "", string icon = null)
        {
            return new URLElement()
            {
                Label = Label.Create(text, tooltip, icon),
                _action = new OpenURLAction()
                {
                    URL = url,
                }
            };
        }
    }
}
