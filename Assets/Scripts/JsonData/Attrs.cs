using System;
using System.Collections.Generic;

namespace JsonData
{
    public class Attrs
    {
        public List<Attr> attrs;
    }

    [Serializable]
    public class Attr
    {
        public int type;
        public int value;
    }
}
