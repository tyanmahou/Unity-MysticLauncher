using System;

namespace Mystic
{
    /// <summary>
    /// サブクラスグループ属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class SubclassGroupAttribute : Attribute
    {

        public SubclassGroupAttribute(string group)
        {
            Group = group;
        }
        public string Group;
    }
}
