using System;

namespace Mystic
{
    [Serializable]
    public class ProcessElement : ActionElement<ProcessStartAction>
    {
        protected override string DefaultTooltip()
        {
            if (_action is null)
            {
                return string.Empty;
            }
            return $"Execute {_action.FileName}";
        }
    }
}
