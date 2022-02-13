using JsonData;
using UnityEngine;

namespace Csv
{
    public class ITEMRESOURCEEx : ITEMRESOURCE, ICsvExtend
    {
        public CommonTypeValues effects;

        public void Parse()
        {
            if (JsonUtil.IsJsonFormat(effect))
            {
                effects = JsonUtility.FromJson<CommonTypeValues>(JsonUtil.AppendJsonName("typeValues", effect));
                effects.Parse();
                effect = null;
            }
        }
    }
}
