using JsonData;
using UnityEngine;

namespace Csv
{
    public class TREASUREBOXRESOURCEEx : TREASUREBOXRESOURCE, ICsvExtend
    {
        public CommonTypeValues showRewards;

        public void Parse()
        {
            if (JsonUtil.IsJsonFormat(showReward))
            {
                showRewards = JsonUtility.FromJson<CommonTypeValues>(JsonUtil.AppendJsonName("typeValues", showReward));
                showRewards.Parse();
                showReward = null;
            }
        }
    }
}
