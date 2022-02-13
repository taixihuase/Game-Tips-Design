using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class CurrencyCfgManager : AbstractCfgManager
    {
        private static CurrencyCfgManager inst;
        public static CurrencyCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new CurrencyCfgManager();
            }
            return inst;
        }

        private Dictionary<int, CURRENCYRESOURCE> map = null;
        private Dictionary<string, CURRENCYRESOURCE> typeDict = null;

        public CurrencyCfgManager()
        {
            map = CreateCsv<CURRENCYRESOURCE>("currencyresource");
            typeDict = new Dictionary<string, CURRENCYRESOURCE>();

            foreach(var item in map.Values)
            {
                typeDict.Add(item.type, item);
            }
        }

        public CURRENCYRESOURCE GetItemById(int id)
        {
            CURRENCYRESOURCE item = null;
            map.TryGetValue(id, out item);
            return item;
        }

        public CURRENCYRESOURCE GetItemByType(string type)
        {
            CURRENCYRESOURCE item = null;
            typeDict.TryGetValue(type, out item);
            return item;
        }
    }
}
