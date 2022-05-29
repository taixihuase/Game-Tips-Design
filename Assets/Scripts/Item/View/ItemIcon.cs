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
        private Image quality;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private TextMeshProUGUI num;
        [SerializeField]
        private Image binding;

        protected BaseItemData itemData;

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
            return CreateTestTipData(itemData);
        }

        private (BaseItemData, BaseItemData, BaseItemData) CreateTestTipData(BaseItemData itemData)
        {
            var tipData = new ItemTipData();
            itemData.tipData = tipData;

            tipData.pos = new Vector2(0, 400);
            tipData.anchor = Vector2.one;
            tipData.pivot = Vector2.one;

            BaseItemData idata2 = null;
            BaseItemData idata3 = null;

            int id = itemData.itemId;
            if (id == 10001)
            {
                itemData.serverData = new AbstractItem();
                tipData.pos = new Vector2(400, 400);
                tipData.canOperate = true;

                idata2 = new BaseItemData(10002, 1);
                tipData = new ItemTipData();
                tipData.isCompareLeftPart = true;
                idata2.tipData = tipData;

                idata3 = new BaseItemData(10003, 1);
                tipData = new ItemTipData();
                tipData.isCompareLeftPart = true;
                idata3.tipData = tipData;
            }
            if (id == 20002)
            {
                itemData.serverData = new AbstractItem();
                itemData.serverData.itemId = id;
                itemData.serverData.num = itemData.num;

                itemData.tipData.canOperate = true;
            }
            if (id == 60001)
            {
                itemData.serverData = new AbstractItem();
                itemData.serverData.itemId = id;
                itemData.serverData.num = itemData.num;

                itemData.tipData.pos = new Vector2(-600, 400);
                tipData.anchor = Vector2.up;
                tipData.pivot = Vector2.up;
                itemData.tipData.canOperate = true;
            }
            if (id == 70002)
            {
                itemData.serverData = new AbstractItem();
                itemData.serverData.itemId = id;
                itemData.serverData.currencyType = "GOLD";

                itemData.tipData.canOperate = true;
            }


            return (itemData, idata2, idata3);
        }

        public override void Clear()
        {

        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            ItemTipManager.Inst().OpenTipView(GenerateDatas(itemData));
        }
    }
}
