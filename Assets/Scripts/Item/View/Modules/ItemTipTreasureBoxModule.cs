using Core;
using Core.UI;
using CsvManager;
using Item.Enum;
using Item.Model;
using System;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipTreasureBoxModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Item;

        [SerializeField]
        private UIGrid itemGrid;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Box)
            {
                if (subModuleType == ItemTipModuleType.ItemModuleType.Preview)
                {
                    itemGrid.Init("ui/window/itemtip/ui_itemtipview_previewboxitem", OnSetDataFinished);
                }
                else if (subModuleType == ItemTipModuleType.ItemModuleType.Selectable)
                {
                    itemGrid.Init("ui/window/itemtip/ui_itemtipview_selectableboxitem", OnSetDataFinished);
                }

                var boxId = Convert.ToInt32(itemData.itemRes.effects.typeValues[0].value);
                var boxRes = TreasureBoxCfgManager.Inst().GetItemById(boxId);
                if (boxRes?.rewardItemDatas?.Count > 0)
                {
                    for (int i = 0; i < boxRes.rewardItemDatas.Count; i++)
                    {
                        BaseItemData idata = boxRes.rewardItemDatas[i];
                        if (idata.tipData == null)
                        {
                            idata.tipData = new ItemTipData();
                        }
                        idata.tipData.isAdditionalPart = true;
                    }
                    itemGrid.SetData(boxRes.rewardItemDatas);
                }
            }

            IsValid = tipType == ItemTipType.Box && itemGrid.GetData()?.Count > 0;
            if (!IsValid)
            {
                OnSetDataFinished();
            }
        }

        protected override void Clear()
        {
            base.Clear();

            itemGrid.Clear();
        }
    }
}
