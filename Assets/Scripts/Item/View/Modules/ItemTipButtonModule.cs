using Core;
using Core.UI;
using Item.Enum;
using Item.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipButtonModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Button;

        [SerializeField]
        private UITable btnTable;
        [SerializeField]
        private Button useBtn;
        [SerializeField]
        private Button sellBtn;
        [SerializeField]
        private Button openBtn;
        [SerializeField]
        private Button equipBtn;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            if (itemData.serverData != null && itemData.tipData.canOperate && !itemData.tipData.isAdditionalPart)
            {
                IsValid = true;

                ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
                sellBtn.gameObject.SetActive(itemData.itemRes.price > 0);
                openBtn.gameObject.SetActive(tipType == ItemTipType.Box);
                equipBtn.gameObject.SetActive(tipType == ItemTipType.Equip);
                useBtn.gameObject.SetActive(tipType != ItemTipType.Box && tipType != ItemTipType.Equip);

                btnTable.Reposition();
            }

            OnSetDataFinished();
        }
    }
}
