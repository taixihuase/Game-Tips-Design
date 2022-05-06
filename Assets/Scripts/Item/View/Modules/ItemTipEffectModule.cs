using Item.Enum;
using Item.Model;
using TMPro;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipEffectModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Effect;

        [SerializeField]
        private TextMeshProUGUI effectDescText;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            effectDescText.text = itemData.itemRes.effectDesc;

            IsValid = !string.IsNullOrEmpty(itemData.itemRes.effect);
            OnSetDataFinished();
        }
    }
}
