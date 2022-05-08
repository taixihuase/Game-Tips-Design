using Core.UI;
using CsvManager;
using JsonData;
using TMPro;
using UnityEngine;
using System;

namespace Item.View.Modules
{
    public class ItemTipAttrItem : UILayoutItem
    {
        [SerializeField]
        private TextMeshProUGUI attrText;

        public override void SetData(object data)
        {
            Attr attr = data as Attr;
            if (attr != null)
            {
                var attrRes = AttributeCfgManager.Inst().GetItemById(attr.type);
                string val = attrRes.isWPercentage ? Convert.ToDouble(attr.value / 10000f).ToString("P") : attr.value.ToString();
                attrText.text = attrRes.displayName + " +" + val;
            }
        }
    }
}
