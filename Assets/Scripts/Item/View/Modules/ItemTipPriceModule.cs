using Item.Enum;
using Item.Model;
using TMPro;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipPriceModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Price;

        [SerializeField]
        private TextMeshProUGUI priceText;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            priceText.text = itemData.itemRes.price.ToString();

            IsValid = itemData.itemRes.price > 0;
            OnSetDataFinished();
        }

        public override float GetLastModuleToBottomSpacing()
        {
            return 4.0f;
        }
    }
}
