using Core.UI;
using CsvManager;
using JsonData;
using TMPro;
using UnityEngine;
using System;
using Core.Character;

namespace Item.View.Modules
{
    public class ItemTipDemandAttrItem : UILayoutItem
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
                var pInfo = CharacterMgr.Inst().PlayerInfo;
                long pVal;
                if (pInfo.attrs.TryGetValue(attr.type, out pVal))
                {
                    if (pVal >= attr.value)
                    {
                        attrText.text = attrRes.displayName + "：" + val;
                    }
                    else
                    {
                        attrText.text = attrRes.displayName + "：<#de2524>" + val + "</color>";
                    }
                }
                else
                {
                    attrText.text = attrRes.displayName + "：<#de2524>" + val + "</color>";
                }
            }
        }
    }
}
