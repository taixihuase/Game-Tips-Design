using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class ItemCfgManager : AbstractCfgManager
    {
        private static ItemCfgManager inst;
        public static ItemCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new ItemCfgManager();
            }
            return inst;
        }

        private Dictionary<int, ITEMRESOURCEEx> map = null;

        public ItemCfgManager()
        {
            map = CreateCsv<ITEMRESOURCEEx>("itemresource");
            foreach (var item in map.Values)
            {
                item.Parse();
            }
        }

        public ITEMRESOURCEEx GetItemById(int id)
        {
            ITEMRESOURCEEx item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
