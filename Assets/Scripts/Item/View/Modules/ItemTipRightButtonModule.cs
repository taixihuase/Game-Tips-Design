using Core;
using Core.UI;
using Item.Enum;
using Item.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipRightButtonModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.RightButton;

        [SerializeField]
        private UIGrid btnGrid;
        [SerializeField]
        private Button detailBtn;
        [SerializeField]
        private Button displayBtn;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            if (!itemData.tipData.isAdditionalPart)
            {
                ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
                if (tipType == ItemTipType.Equip || tipType == ItemTipType.Skill || tipType == ItemTipType.Mount)
                {
                    btnGrid.Reposition();
                    IsValid = true;

                    detailBtn.onClick.AddListener(OnDetailBtnClick);
                    displayBtn.onClick.AddListener(OnDisplayBtnClick);
                }
            }
            OnSetDataFinished();
        }

        private void OnDetailBtnClick()
        {
            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Equip)
            {
                parentView?.HideCurrentValidModules(2, new int[] { 
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.BaseInfo,
                    ItemTipModuleType.Demand,
                    ItemTipModuleType.Attr + ItemTipModuleType.AttrModuleType.Base,
                    ItemTipModuleType.Attr + ItemTipModuleType.AttrModuleType.Addition,
                    ItemTipModuleType.Desc,
                    ItemTipModuleType.Price,
                    ItemTipModuleType.Button});
            }
            else if (tipType == ItemTipType.Skill)
            {
                parentView?.HideCurrentValidModules(2, new int[] {
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.BaseInfo,
                    ItemTipModuleType.Demand,
                    ItemTipModuleType.Skill,
                    ItemTipModuleType.Effect,
                    ItemTipModuleType.Desc,
                    ItemTipModuleType.Price,
                    ItemTipModuleType.Button});
            }
            else if (tipType == ItemTipType.Mount)
            {
                parentView?.HideCurrentValidModules(2, new int[] {
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.BaseInfo,
                    ItemTipModuleType.Demand,
                    ItemTipModuleType.Effect,
                    ItemTipModuleType.Desc,
                    ItemTipModuleType.Price,
                    ItemTipModuleType.Button});
            }
        }

        private void OnDisplayBtnClick()
        {
            ItemTipType tipType = EnumUtil.GetEnumByDescription<ItemTipType>(itemData.itemRes.type);
            if (tipType == ItemTipType.Equip)
            {
                parentView?.HideCurrentValidModules(1, new int[] {
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.Display});
            }
            else if (tipType == ItemTipType.Skill)
            {
                parentView?.HideCurrentValidModules(1, new int[] {
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.Display});
            }
            else if (tipType == ItemTipType.Mount)
            {
                parentView?.HideCurrentValidModules(1, new int[] {
                    ItemTipModuleType.Header,
                    ItemTipModuleType.RightButton});

                parentView?.ShowTargetVaildModules(new int[] {
                    ItemTipModuleType.Display});
            }
        }

        protected override void Clear()
        {
            base.Clear();

            detailBtn.onClick.RemoveListener(OnDetailBtnClick);
            displayBtn.onClick.RemoveListener(OnDisplayBtnClick);
        }
    }
}
