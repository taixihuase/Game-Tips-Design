using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class TreasureBoxCfgManager : AbstractCfgManager
    {
        private static TreasureBoxCfgManager inst;
        public static TreasureBoxCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new TreasureBoxCfgManager();
            }
            return inst;
        }

        private Dictionary<int, TREASUREBOXRESOURCEEx> map = null;

        public TreasureBoxCfgManager()
        {
            map = CreateCsv<TREASUREBOXRESOURCEEx>("treasureboxresource");
            foreach (var item in map.Values)
            {
                item.Parse();
            }
        }

        public TREASUREBOXRESOURCEEx GetItemById(int id)
        {
            TREASUREBOXRESOURCEEx item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
