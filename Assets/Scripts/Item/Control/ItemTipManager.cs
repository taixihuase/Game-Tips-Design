using Item.Enum;
using Item.Model;
using Item.View;
using Item.View.Modules;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Item.Control
{
    public class ItemTipManager
    {
        private static ItemTipManager inst;
        public static ItemTipManager Inst()
        {
            if (inst == null)
            {
                inst = new ItemTipManager();
            }
            return inst;
        }

        private Dictionary<string, ItemTipType> registedTipTypes = new Dictionary<string, ItemTipType>();
        private Dictionary<ItemTipType, List<int>> registedTipModules = new Dictionary<ItemTipType, List<int>>();
        private (BaseItemData it1, BaseItemData it2, BaseItemData it3) itemDatas;

        public ItemTipManager()
        {
            RegistTipTypes();
            RegistTipModules();
        }

        private void RegistTipTypes()
        {
            RegistType("NORMAL", ItemTipType.Item);
            RegistType("EQUIP", ItemTipType.Equip);
            RegistType("SKILLBOOK", ItemTipType.Skill);
            RegistType("MOUNT", ItemTipType.Mount);
            RegistType("TREASUREBOX", ItemTipType.Box);
            RegistType("CURRENCY", ItemTipType.Currency);
        }

        private void RegistType(string itemType, ItemTipType tipType)
        {
            registedTipTypes.Add(itemType, tipType);
        }

        private void RegistTipModules()
        {
            RegistModules(ItemTipType.Item,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button);

            RegistModules(ItemTipType.Equip,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Attr + ItemTipModuleType.AttrModuleType.Base,
                ItemTipModuleType.Attr + ItemTipModuleType.AttrModuleType.Addition,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button,
                ItemTipModuleType.RightButton,
                ItemTipModuleType.Display);

            RegistModules(ItemTipType.Skill,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Skill,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button,
                ItemTipModuleType.RightButton,
                ItemTipModuleType.Display);

            RegistModules(ItemTipType.Mount,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button,
                ItemTipModuleType.RightButton,
                ItemTipModuleType.Display);

            RegistModules(ItemTipType.Box,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Item + ItemTipModuleType.ItemModuleType.Preview,
                ItemTipModuleType.Item + ItemTipModuleType.ItemModuleType.Selectable,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button);

            RegistModules(ItemTipType.Currency,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Button);
        }

        private void RegistModules(ItemTipType tipType, params int[] modules)
        {
            registedTipModules.Add(tipType, modules.ToList());
        }

        public void OpenTipView((BaseItemData it1, BaseItemData it2, BaseItemData it3) itemDatas)
        {
            this.itemDatas = itemDatas;
            OpenTipView(itemDatas.it1, itemDatas.it2, itemDatas.it3);
        }

        public void OpenTipView(BaseItemData itemData1, BaseItemData itemData2 = null, BaseItemData itemData3 = null)
        {
            itemData1.tipData.isCompare = false;

            if (itemData2 != null)
            {
                itemData2.tipData.additionalPartIndex = 1;
                itemData2.tipData.isAdditionalPart = true;
                //对比tip以原始tip坐标做偏移
                itemData2.tipData.pos = itemData1.tipData.pos;
                itemData2.tipData.anchor = itemData1.tipData.anchor;
                itemData2.tipData.pivot = itemData1.tipData.pivot;
            }
            
            if (itemData3 != null)
            {
                itemData3.tipData.additionalPartIndex = 2;
                itemData3.tipData.isAdditionalPart = true;
                //对比tip放在同一侧
                itemData3.tipData.isCompareLeftPart = itemData2.tipData.isCompareLeftPart;
                //对比tip以原始tip坐标做偏移
                itemData3.tipData.pos = itemData1.tipData.pos;
                itemData3.tipData.anchor = itemData1.tipData.anchor;
                itemData3.tipData.pivot = itemData1.tipData.pivot;
            }

            OpenTipView(itemData1);
            OpenTipView(itemData2);
            OpenTipView(itemData3);
        }

        public void OpenTipView(BaseItemData itemData)
        {
            if (itemData == null)
            {
                return;
            }

            if (!itemData.tipData.isAdditionalPart)
            {
                ItemTipPool.Inst().RecycleUsingTips();
            }
            else
            {
                ItemTipPool.Inst().RecycleUsingTips(itemData.tipData.additionalPartIndex);
                ItemTipPool.Inst().RecycleUsingTips(itemData.tipData.additionalPartIndex + 1);
            }
            int usingCnt = ItemTipPool.Inst().GetUsingTipCount();
            if (usingCnt >= 3)
            {
                Debug.LogError("已经显示最大数量的tips");
                return;
            }

            ItemTipType tipType = GetTipType(itemData);
            ItemTipView tipView = ItemTipPool.Inst().PopTip();
            List<int> modules;
            if (registedTipModules.TryGetValue(tipType, out modules))
            {
                for (int i = 0; i < modules.Count; i++)
                {
                    int moduleType = modules[i];
                    int subModuleType = moduleType % 100;
                    int baseModuleType = moduleType - subModuleType;
                    ItemTipModule module = ItemTipPool.Inst().PopModule(baseModuleType);
                    module.subModuleType = subModuleType;
                    tipView.AddModule(module);
                }
            }
            tipView.SetData(itemData);
        }

        private ItemTipType GetTipType(BaseItemData itemData)
        {
            string tipTypeStr = itemData.itemRes.type;
            ItemTipType tipType;
            if (registedTipTypes.TryGetValue(tipTypeStr, out tipType))
            {
                return tipType;
            }
            return ItemTipType.Item;
        }
    }
}
