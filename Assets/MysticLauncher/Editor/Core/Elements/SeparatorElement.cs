using System;

namespace Mystic
{
    [Serializable]
    [SubclassGroup("Layout")]
    public class SeparatorElement : IElement
    {
        public void OnGUI()
        {
            EditorGUIUtil.DrawSeparator();
        }
        public override string ToString()
        {
            return "Seperator";
        }
    }
}
