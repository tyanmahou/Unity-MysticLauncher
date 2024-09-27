using System;

namespace Mystic
{
    [Serializable]
    public class MenuItemElement : ActionElement<MenuItemAction>
    {
        protected override string DefaultTooltip()
        {
            return _action?.ItemName ?? string.Empty;
        }
    }
}
