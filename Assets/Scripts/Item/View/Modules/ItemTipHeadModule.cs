using Core;
using Item.Enum;
using Item.Model;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using Core.UI;
using TMPro;

namespace Item.View.Modules
{
    public class ItemTipHeadModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Header;

        [SerializeField]
        private Image quality;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Image binding;
        [SerializeField]
        private TextMeshProUGUI itemName;

        public override bool IsValid()
        {
            return true;
        }

        public override float Relayout()
        {
            return base.Relayout();
        }

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            itemName.text = itemData.itemRes.name;
            binding.gameObject.SetActive(itemData.itemRes.binding);

            SpriteAtlas atlas = AssetLoader.LoadAsset<SpriteAtlas>("ui/atlas/atlas_comm.spriteatlasv2");
            if (atlas != null)
            {
                quality.overrideSprite = atlas.GetSprite(GetQualitySp(itemData.itemRes.quality));
            }
            atlas = AssetLoader.LoadAsset<SpriteAtlas>("ui/atlas/atlas_icon.spriteatlasv2");
            if (atlas != null)
            {
                icon.overrideSprite = atlas.GetSprite(itemData.itemRes.icon);
            }

            OnSetDataFinished();
        }

        private string GetQualitySp(int quality)
        {
            switch (quality)
            {
                case 1: return "comm_sp_0002";
                case 2: return "comm_sp_0003";
                case 3: return "comm_sp_0004";
                case 4: return "comm_sp_0005";
                case 5: return "comm_sp_0006";
                case 6: return "comm_sp_0007";
                case 7: return "comm_sp_0008";
                default: return "comm_sp_0002";
            }
        }
    }
}
