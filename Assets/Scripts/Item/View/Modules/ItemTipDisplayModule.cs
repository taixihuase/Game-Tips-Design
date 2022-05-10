using Core;
using Item.Enum;
using Item.Model;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipDisplayModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.Display;

        [SerializeField]
        private Image displayIcon;

        public override void SetData(BaseItemData itemData)
        {
            var atlas = AssetLoader.LoadAsset<SpriteAtlas>("ui/atlas/atlas_icon.spriteatlasv2");
            if (atlas != null)
            {
                displayIcon.overrideSprite = atlas.GetSprite(itemData.itemRes.icon);
            }
            IsValid = true;
            OnSetDataFinished();
        }
    }
}
