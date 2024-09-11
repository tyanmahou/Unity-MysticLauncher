using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace Mystic
{
    public class SubclassSelectorDropdown : AdvancedDropdown
    {
        public SubclassSelectorDropdown(AdvancedDropdownState state, string title, string[] select, System.Action<int> onItemSelected) : base(state)
        {
            this._title = title;
            this._select = select;
            this._onItemSelected = onItemSelected;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            // ルートアイテムの作成
            var root = new AdvancedDropdownItem(this._title);
            // 選択肢の追加
            for (int i = 0; i < _select.Length; i++)
            {
                var item = new AdvancedDropdownItem(_select[i]);
                root.AddChild(item);

                _idMap.Add(item.id, i);
            }

            return root;
        }

        // アイテムが選択された際の処理
        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);

            // 選択したアイテムのインデックスを取得してコールバック
            int selectedIndex = _idMap[item.id];
            _onItemSelected?.Invoke(selectedIndex);
        }
        private string _title;
        private string[] _select;
        private System.Action<int> _onItemSelected;
        private Dictionary<int, int> _idMap = new Dictionary<int, int>();

    }
}
