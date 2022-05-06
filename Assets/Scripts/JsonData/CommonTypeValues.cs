using System;
using System.Collections.Generic;

namespace JsonData
{
    public class CommonTypeValues
    {
        public List<CommonTypeValue> typeValues;

        public void Parse()
        {
            if (typeValues != null)
            {
                foreach(var tv in typeValues)
                {
                    tv.Parse();
                }
            }
        }
    }

    [Serializable]
    public class CommonTypeValue
    {
        public string type;
        public string value;
        public string subType;
        public long subValue;

        private const char splitChar = '_';

        public const string Item = "Item";
        public const string Skill = "Skill";
        public const string Currency = "Currency";

        public void Parse()
        {
            if (value.Contains(splitChar))
            {
                var strs = value.Split(splitChar);
                subType = strs[0];
                subValue = Convert.ToInt64(strs[1]);
            }
        }
    }
}
