using UnityEngine;

namespace Mystic
{
    public class MenuItemPickerAttribute : PropertyAttribute
    {
        public MenuItemPickerAttribute(
            bool freeInput = true
            )
        {
            this.FreeInput = freeInput;
        }
        public bool FreeInput;
    }
}
