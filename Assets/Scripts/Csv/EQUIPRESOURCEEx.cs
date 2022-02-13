using JsonData;
using System;
using UnityEngine;

namespace Csv
{
    public class EQUIPRESOURCEEx : EQUIPRESOURCE, ICsvExtend
    {
        public Attrs demandAttrs;
        public Attrs baseAttrs;
        public Attrs addAttrs;

        public void Parse()
        {
            if (JsonUtil.IsJsonFormat(demand))
            {
                demandAttrs = JsonUtility.FromJson<Attrs>(JsonUtil.AppendJsonName("attrs", demand));
                demand = null;
            }
            if (JsonUtil.IsJsonFormat(baseAttr))
            {
                baseAttrs = JsonUtility.FromJson<Attrs>(JsonUtil.AppendJsonName("attrs", baseAttr));
                baseAttr = null;
            }
            if (JsonUtil.IsJsonFormat(addAttr))
            {
                addAttrs = JsonUtility.FromJson<Attrs>(JsonUtil.AppendJsonName("attrs", addAttr));
                addAttr = null;
            }
        }
    }
}
