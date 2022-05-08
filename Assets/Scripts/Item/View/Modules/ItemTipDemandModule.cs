using Core;
using Core.UI;
using CsvManager;
using Item.Enum;
using Item.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipDemandModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Demand;

        [SerializeField]
        private UITable demandTable;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Equip)
            {
                var equipRes = EquipCfgManager.Inst().GetItemById(itemData.itemId);
                if (equipRes != null)
                {
                    var attrs = equipRes.demandAttrs;
                    demandTable.Init("ui/window/itemtip/ui_itemtipview_demandattritem.prefab", OnSetDataFinished);
                    if (attrs?.IsEmpty() == false)
                    {
                        IsValid = attrs.attrs.Count > 0;
                        demandTable.SetData(attrs.attrs);
                    }
                }
            }

            if (!IsValid)
            {
                OnSetDataFinished();
            }
        }

        public override float Relayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(demandTable.GetComponent<RectTransform>());
            return base.Relayout();
        }

        protected override void Clear()
        {
            base.Clear();

            demandTable.Clear();
        }
    }
}
