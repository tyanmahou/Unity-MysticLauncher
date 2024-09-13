using System;

namespace Mystic
{
    [Serializable]
    public class SettingServiceElement : ActionElement<OpenSettingService>
    {
        protected override string DefaultTooltip()
        {
            if (_action is null)
            {
                return string.Empty;
            }
            return $"Open {_action.Path.SettingPath}";
        }
    }
}
