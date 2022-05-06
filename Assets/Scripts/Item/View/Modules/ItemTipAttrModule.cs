using Core;
using Core.UI;
using CsvManager;
using Item.Enum;
using Item.Model;
using JsonData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipAttrModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Attr;

        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private UIGrid attrGrid;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            if (subModuleType == ItemTipModuleType.AttrModuleType.Base)
            {
                title.text = "基础属性";
            }
            else if (subModuleType == ItemTipModuleType.AttrModuleType.Addition)
            {
                title.text = "附加属性";
            }
            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Equip)
            {
                var equipRes = EquipCfgManager.Inst().GetItemById(itemData.itemId);
                if (equipRes != null)
                {
                    attrGrid.Init("ui/window/itemtip/ui_itemtipview_attritem", OnSetDataFinished);
                    Attrs attrs = null;
                    if (subModuleType == ItemTipModuleType.AttrModuleType.Base)
                    {
                        attrs = equipRes.baseAttrs;
                    }
                    else if (subModuleType == ItemTipModuleType.AttrModuleType.Addition)
                    {
                        attrs = equipRes.addAttrs;
                    }
                    if (attrs?.IsEmpty() == false)
                    {
                        attrGrid.SetData(attrs.attrs);
                    }
                }
            }

            IsValid = attrGrid.GetData()?.Count > 0;
            if (!IsValid)
            {
                OnSetDataFinished();
            }
        }
    }
}
