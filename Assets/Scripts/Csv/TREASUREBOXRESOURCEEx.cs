using Item.Model;
using JsonData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Csv
{
    public class TREASUREBOXRESOURCEEx : TREASUREBOXRESOURCE, ICsvExtend
    {
        public CommonTypeValues showRewards;
        public List<BaseItemData> rewardItemDatas;

        public void Parse()
        {
            if (JsonUtil.IsJsonFormat(showReward))
            {
                showRewards = JsonUtility.FromJson<CommonTypeValues>(JsonUtil.AppendJsonName("typeValues", showReward));
                showRewards.Parse();
                showReward = null;

                if (showRewards?.typeValues?.Count > 0)
                {
                    rewardItemDatas = new List<BaseItemData>();
                    for (int i = 0; i < showRewards.typeValues.Count; i++)
                    {
                        var tv = showRewards.typeValues[i];
                        BaseItemData idata = new BaseItemData();
                        if (tv.type == CommonTypeValue.Item)
                        {
                            idata.SetData(Convert.ToInt32(tv.subType), tv.subValue);
                        }
                        else if (tv.type == CommonTypeValue.Currency)
                        {
                            idata.SetData(tv.subType, tv.subValue);
                        }
                        rewardItemDatas.Add(idata);
                    }
                }
            }
        }
    }
}
