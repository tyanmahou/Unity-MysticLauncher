using UnityEngine;

namespace Mystic
{
    /// <summary>
    /// サブクラス選択属性
    /// </summary>
    public class SubclassSelectorAttribute : PropertyAttribute
    {

        public SubclassSelectorAttribute(bool includeMono = false)
        {
            _includeMono = includeMono;
        }

        public bool IsIncludeMono()
        {
            return _includeMono;
        }

        private bool _includeMono;
    }
}
