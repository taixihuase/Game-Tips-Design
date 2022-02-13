using JsonData;
using System;
using UnityEngine;

namespace Csv
{
    public class SKILLRESOURCEEx : SKILLRESOURCE, ICsvExtend
    {
        public CommonTypeValues activeDemands;
        public CommonTypeValues upgradeDemands;

        public void Parse()
        {
            if (JsonUtil.IsJsonFormat(activeDemand))
            {
                activeDemands = JsonUtility.FromJson<CommonTypeValues>(JsonUtil.AppendJsonName("typeValues", activeDemand));
                activeDemands.Parse();
                activeDemand = null;
            }
            if (JsonUtil.IsJsonFormat(upgradeDemand))
            {
                upgradeDemands = JsonUtility.FromJson<CommonTypeValues>(JsonUtil.AppendJsonName("typeValues", upgradeDemand));
                upgradeDemands.Parse();
                upgradeDemand = null;
            }
        }
    }
}
