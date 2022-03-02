using Core;
using Core.UI;
using Item.Control;
using Item.Model;
using System;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Item.View
{
    public class ItemIcon : UILayoutItem
    {
        [SerializeField]
        private int size = 80;
        [SerializeField]
        private Image quality;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Text num;
        [SerializeField]
        private Image binding;
        [SerializeField]
        private Button button;

        private BaseItemData itemData;

        protected bool isAddLis;

        public override void SetData(object data)
        {
            itemData = data as BaseItemData;
            if (itemData == null)
            {
                return;
            }

            AddOrRemoveLis(true);

            SpriteAtlas atlas = AssetLoader.LoadAsset<SpriteAtlas>("atlas_comm");
            if (atlas != null)
            {
                quality.overrideSprite = atlas.GetSprite(GetQualitySp(itemData.itemRes.quality));
            }

            atlas = AssetLoader.LoadAsset<SpriteAtlas>("atlas_icon");
            if (atlas != null)
            {
                icon.overrideSprite = atlas.GetSprite(itemData.itemRes.icon);
            }
            num.text = itemData.num.ToString();
            binding.gameObject.SetActive(itemData.itemRes.binding);
        }

        private string GetQualitySp(int quality)
        {
            switch(quality)
            {
                case 1:return "comm_sp_0002";
                case 2:return "comm_sp_0003";
                case 3:return "comm_sp_0004";
                case 4:return "comm_sp_0005";
                case 5:return "comm_sp_0006";
                case 6:return "comm_sp_0007";
                case 7:return "comm_sp_0008";
                default:return "comm_sp_0002";
            }
        }

        protected void AddOrRemoveLis(bool isAdd)
        {
            if (isAddLis != isAdd)
            {
                isAddLis = isAdd;
                if(isAdd)
                {
                    button.onClick.AddListener(OnClick);
                }
                else
                {
                    button.onClick.RemoveListener(OnClick);
                }
            }
        }

        private void OnClick()
        {
            ItemTipManager.Inst().OpenTipView(GenerateDatas(itemData));
        }

        protected virtual ValueTuple<BaseItemData, BaseItemData, BaseItemData> GenerateDatas(BaseItemData itemData)
        {
            ValueTuple<BaseItemData, BaseItemData, BaseItemData> vt = (itemData, null, null);
            return vt;
        }

        public override void Clear()
        {
            AddOrRemoveLis(false);
        }
    }
}
