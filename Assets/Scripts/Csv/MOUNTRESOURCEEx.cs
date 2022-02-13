using JsonData;
using System;
using UnityEngine;

namespace Csv
{
    public class MOUNTRESOURCEEx : MOUNTRESOURCE, ICsvExtend
    {
        public CommonTypeValues activeDemands;
        public CommonTypeValues upgradeDemands;
        public Attrs baseAttrs;

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
            if (JsonUtil.IsJsonFormat(baseAttr))
            {
                baseAttrs = JsonUtility.FromJson<Attrs>(JsonUtil.AppendJsonName("attrs", baseAttr));
                baseAttr = null;
            }
        }
    }
}
