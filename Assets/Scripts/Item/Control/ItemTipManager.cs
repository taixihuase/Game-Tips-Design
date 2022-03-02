using Item.Enum;
using Item.Model;
using Item.View;
using System;
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
        private ValueTuple<BaseItemData, BaseItemData, BaseItemData> itemDatas;

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
                ItemTipModuleType.Suit + ItemTipModuleType.SuitModuleType.Base,
                ItemTipModuleType.Suit + ItemTipModuleType.SuitModuleType.Forge,
                ItemTipModuleType.Suit + ItemTipModuleType.SuitModuleType.Element,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button);

            RegistModules(ItemTipType.Skill,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Skill,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button);

            RegistModules(ItemTipType.Mount,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Skill,
                ItemTipModuleType.Effect,
                ItemTipModuleType.Desc,
                ItemTipModuleType.Price,
                ItemTipModuleType.Button);

            RegistModules(ItemTipType.Box,
                ItemTipModuleType.Header,
                ItemTipModuleType.BaseInfo,
                ItemTipModuleType.Demand,
                ItemTipModuleType.Item,
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

        public void OpenTipView(ValueTuple<BaseItemData, BaseItemData, BaseItemData> itemDatas)
        {
            OpenTipView(itemDatas.Item1, itemDatas.Item2, itemDatas.Item3);
        }

        public void OpenTipView(BaseItemData itemData1, BaseItemData itemData2 = null, BaseItemData itemData3 = null)
        {
            OpenTipView(itemData1);
            if (itemData2 != null)
            {
                itemData2.tipData.isAdditionalPart = true;
                OpenTipView(itemData2);
            }
            
            if (itemData3 != null)
            {
                itemData3.tipData.isAdditionalPart = true;
                OpenTipView(itemData3);
            }
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
