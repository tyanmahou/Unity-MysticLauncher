using System;

namespace Mystic
{
    /// <summary>
    /// ラベル アイコン付き
    /// </summary>
    [Serializable]
    public struct Label
    {
        public static Label Create(string text, string tooltip = "", string icon = null)
        {
            return new Label()
            {
                Text = text,
                Tooltip = tooltip,
                Icon = Icon.CreateUnityIcon(icon)
            };
        }
        public string Text;
        public string Tooltip;
        public Icon Icon;
    }
}
