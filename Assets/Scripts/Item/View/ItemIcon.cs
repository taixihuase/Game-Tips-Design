using Core;
using Core.UI;
using Item.Control;
using Item.Model;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Item.View
{
    public class ItemIcon : UILayoutItem, IPointerClickHandler
    {
        [SerializeField]
        private int size = 86;
        [SerializeField]
        private Image quality;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI num;
        [SerializeField]
        private Image binding;

        private BaseItemData itemData;

        protected bool isAddLis;

        public override void SetData(object data)
        {
            itemData = data as BaseItemData;
            if (itemData == null)
            {
                return;
            }

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
            num.text = itemData.num.ToString();
            binding.gameObject.SetActive(itemData.itemRes.binding);
        }

        private string GetQualitySp(int quality)
        {
            switch(quality)
            {
                case 1:return "comm_sp_0019";
                case 2:return "comm_sp_0012";
                case 3:return "comm_sp_0013";
                case 4:return "comm_sp_0014";
                case 5:return "comm_sp_0015";
                case 6:return "comm_sp_0016";
                case 7:return "comm_sp_0017";
                default:return "comm_sp_0019";
            }
        }

        protected virtual (BaseItemData, BaseItemData, BaseItemData) GenerateDatas(BaseItemData itemData)
        {
            CreateTestTipData(itemData);
            return (itemData, null, null);
        }

        private void CreateTestTipData(BaseItemData itemData)
        {
            var tipData = new ItemTipData();
            itemData.tipData = tipData;

            tipData.pos = new Vector2(100, 100);
            tipData.anchor = Vector2.right;
            tipData.pivot = Vector2.right;

        }

        public override void Clear()
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemTipManager.Inst().OpenTipView(GenerateDatas(itemData));
        }
    }
}
