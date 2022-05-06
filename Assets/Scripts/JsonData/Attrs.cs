using System;
using System.Collections.Generic;

namespace JsonData
{
    public class Attrs
    {
        public List<Attr> attrs;

        public bool IsEmpty()
        {
            return attrs == null || attrs.Count == 0;
        }
    }

    [Serializable]
    public class Attr
    {
        public int type;
        public long value;

        public Attr()
        {

        }

        public Attr(int type, long value)
        {
            this.type = type;
            this.value = value;
        }
    }
}
