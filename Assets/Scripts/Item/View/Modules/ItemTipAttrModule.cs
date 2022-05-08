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
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipAttrModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Attr;

        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private UITable attrTable;

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
                    attrTable.Init("ui/window/itemtip/ui_itemtipview_attritem.prefab", OnSetDataFinished);
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
                        IsValid = attrs.attrs.Count > 0;
                        attrTable.SetData(attrs.attrs);
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
            LayoutRebuilder.ForceRebuildLayoutImmediate(attrTable.GetComponent<RectTransform>());
            return base.Relayout();
        }

        protected override void Clear()
        {
            base.Clear();

            attrTable.Clear();
        }
    }
}
