using Item.Enum;
using Item.Model;
using TMPro;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipDescModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Desc;

        [SerializeField]
        private TextMeshProUGUI descText;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            descText.text = itemData.itemRes.desc;

            IsValid = !string.IsNullOrEmpty(itemData.itemRes.desc);
            OnSetDataFinished();
        }

        public override float GetLastModuleToBottomSpacing()
        {
            return 4.0f;
        }
    }
}
