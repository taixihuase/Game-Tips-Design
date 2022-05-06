using Core;
using Core.UI;
using CsvManager;
using Item.Enum;
using Item.Model;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipDemandModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Demand;

        [SerializeField]
        private GameObject demandLimitObj;
        [SerializeField]
        private UIGrid demandGrid;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            demandLimitObj.SetActive(tipType == ItemTipType.Equip);
            if (tipType == ItemTipType.Equip)
            {
                var equipRes = EquipCfgManager.Inst().GetItemById(itemData.itemId);
                if (equipRes != null)
                {
                    var attrs = equipRes.demandAttrs;
                    demandGrid.Init("ui/window/itemtip/ui_itemtipview_demandattritem", OnGridReposition);
                    if (attrs?.IsEmpty() == false)
                    {
                        demandGrid.SetData(attrs.attrs);
                    }
                }
            }

            IsValid = demandGrid.GetData()?.Count > 0;
            if (!IsValid)
            {
                OnSetDataFinished();
            }
        }

        private void OnGridReposition()
        {
            OnSetDataFinished();
        }
    }
}
