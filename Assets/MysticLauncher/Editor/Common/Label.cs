using System;

namespace Mystic
{
    /// <summary>
    /// ラベル アイコン付き
    /// </summary>
    [Serializable]
    public struct Label
    {
        public string Text;
        public string Tooltip;
        public Icon Icon;
    }
}
