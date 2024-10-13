using UnityEngine;

namespace Mystic
{
    public class SearchField
    {
        public string OnGUI(string searchString)
        {
            _searchField ??= new();
            return _searchField.OnGUI(searchString, GUILayout.MinWidth(0));
        }
        private UnityEditor.IMGUI.Controls.SearchField _searchField;
    }
}
