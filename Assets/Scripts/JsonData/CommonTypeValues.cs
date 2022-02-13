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
        public int subValue;

        private const char splitChar = '_';

        private const string Item = "Item";
        private const string Skill = "Skill";
        private const string Currency = "Currency";

        public void Parse()
        {
            if (value.Contains(splitChar))
            {
                var strs = value.Split(splitChar);
                subType = strs[0];
                subValue = Convert.ToInt32(strs[1]);
            }
        }
    }
}
