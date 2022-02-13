using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class AttributeCfgManager : AbstractCfgManager
    {
        private static AttributeCfgManager inst;
        public static AttributeCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new AttributeCfgManager();
            }
            return inst;
        }

        private Dictionary<int, ATTRIBUTERESOURCE> map = null;

        public AttributeCfgManager()
        {
            map = CreateCsv<ATTRIBUTERESOURCE>("attributeresource");
        }

        public ATTRIBUTERESOURCE GetItemById(int id)
        {
            ATTRIBUTERESOURCE item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
