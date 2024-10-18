using System.Collections.Generic;
using System.Linq;
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

            // スラッシュを含むものを優先的に表示するようにソート
            var sortedSelect = _select
                .Select((s, i) => (s, i)) // もとのインデックスを残しておく
                .OrderBy(si => si.s == "Null" ? 0 : 1)
                .ThenByDescending(si => si.s.Count(c => c == '/'))
                ;
            // 選択肢の追加
            foreach(var(select, i) in sortedSelect)
            {
                var pathParts = select.Split('/');
                var currentParent = root;

                // パスの各部分ごとにツリーを作成
                foreach (var part in pathParts)
                {
                    // 同じ名前の子アイテムがすでにあるか確認
                    var existingChild = currentParent.children.FirstOrDefault(c => c.name == part);
                    if (existingChild != null)
                    {
                        currentParent = existingChild;  // 既存の子に移動
                    }
                    else
                    {
                        var newChild = new AdvancedDropdownItem(part);
                        currentParent.AddChild(newChild);
                        currentParent = newChild;  // 新しい子を親として次へ
                    }
                }

                // 最終的なアイテムにIDをマッピング
                _idMap.Add(currentParent.id, i);
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
