using Core.UI;
using Item.Enum;
using Item.Model;
using TMPro;
using UnityEngine;

namespace Item.View.Modules
{
    public class ItemTipBaseModule : ItemTipModule
    {
        public override int moduleType => ItemTipModuleType.BaseInfo;

        [SerializeField]
        private UITable layoutTable;
        [SerializeField]
        private GameObject levelLimitObj;
        [SerializeField]
        private TextMeshProUGUI levelLimit;
        [SerializeField]
        private GameObject jobLimitObj;
        [SerializeField]
        private TextMeshProUGUI jobLimit;

        public override void SetData(BaseItemData itemData)
        {
            base.SetData(itemData);

            levelLimitObj.SetActive(itemData.itemRes.level > 1);
            levelLimit.text = "Lv." + itemData.itemRes.level;
            jobLimitObj.SetActive(itemData.itemRes.job > 0);
            jobLimit.text = GetJobName(itemData.itemRes.job);           

            IsValid = 
                itemData.itemRes.level > 1 || itemData.itemRes.job > 0;
            OnSetDataFinished();
        }

        private string GetJobName(int job)
        {
            switch(job)
            {
                case 1: return "战士";
                case 2: return "法师";
                case 3: return "牧师";
                default:return "";
            }
        }

        public override float Relayout()
        {
            layoutTable.Reposition();
            return base.Relayout();
        }
    }
}
