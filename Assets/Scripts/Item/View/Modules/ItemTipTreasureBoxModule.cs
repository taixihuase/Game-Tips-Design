using Core;
using Core.UI;
using CsvManager;
using Item.Enum;
using Item.Model;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipTreasureBoxModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Item;

        [SerializeField]
        private UIGrid itemGrid;

        [SerializeField]
        private ToggleGroup toggleGroup;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Box)
            {
                if (subModuleType == ItemTipModuleType.ItemModuleType.Preview)
                {
                    var boxId = Convert.ToInt32(itemData.itemRes.effects.typeValues[0].value);
                    var boxRes = TreasureBoxCfgManager.Inst().GetItemById(boxId);
                    if (itemData.tipData.canOperate && boxRes.num > 0)
                    {
                        IsValid = false;
                    }
                    else
                    {
                        itemGrid.Init("ui/window/itemtip/ui_itemtipview_previewboxitem.prefab", OnSetDataFinished);
                        SetGridData();
                    }
                }
                else if (subModuleType == ItemTipModuleType.ItemModuleType.Selectable)
                {
                    if (itemData.tipData.canOperate)
                    {
                        itemGrid.Init("ui/window/itemtip/ui_itemtipview_selectableboxitem.prefab", OnSetDataFinished);
                        SetGridData();
                    }
                    else
                    {
                        IsValid = false;
                    }
                }
            }

            if (!IsValid)
            {
                OnSetDataFinished();
            }
        }

        private void SetGridData()
        {
            var boxId = Convert.ToInt32(itemData.itemRes.effects.typeValues[0].value);
            var boxRes = TreasureBoxCfgManager.Inst().GetItemById(boxId);
            if (boxRes?.rewardItemDatas?.Count > 0)
            {
                List<BaseItemData> list = new List<BaseItemData>();
                for (int i = 0; i < boxRes.rewardItemDatas.Count; i++)
                {
                    BaseItemData idata = boxRes.rewardItemDatas[i].Clone();
                    if (idata.tipData == null)
                    {
                        idata.tipData = new ItemTipData();
                    }
                    idata.tipData.pos = itemData.tipData.pos;
                    idata.tipData.pivot = itemData.tipData.pivot;
                    idata.tipData.anchor = itemData.tipData.anchor;
                    idata.tipData.isAdditionalLeftPart = false;
                    idata.tipData.additionalPartIndex = itemData.tipData.additionalPartIndex + 1;
                    list.Add(idata);
                }
                IsValid = true;
                itemGrid.SetData(list);
            }
        }

        protected override void OnSetDataFinished()
        {
            base.OnSetDataFinished();

            if (IsValid && subModuleType == ItemTipModuleType.ItemModuleType.Selectable)
            {
                var itemList = itemGrid.GetItems();
                for (int i = 0; i < itemList.Count; i++)
                {
                    var item = itemList[i] as ItemTipSelectableBoxItem;
                    if (item != null)
                    {
                        item.SelectToggle.group = toggleGroup;
                    }
                }
            }
        }

        protected override void Clear()
        {
            base.Clear();

            itemGrid.Clear();
        }
    }
}
