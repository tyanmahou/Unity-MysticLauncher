using System;

namespace Mystic
{
    [Serializable]
    public class URLElement : ActionElement<OpenUrlAction>
    {
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
