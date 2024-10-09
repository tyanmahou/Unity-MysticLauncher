using System;
using UnityEditor;

namespace Mystic
{
    [Serializable]
    public class MenuItemAction : IToolAction
    {
        [MenuItemPicker]
        public string ItemName;

        public void Execute()
        {
            EditorApplication.ExecuteMenuItem(ItemName);
        }
        public string Tooltip()
        {
            return ItemName ?? string.Empty;
        }
    }
}
