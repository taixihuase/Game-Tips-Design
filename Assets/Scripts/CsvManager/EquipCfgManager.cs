using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class EquipCfgManager : AbstractCfgManager
    {
        private static EquipCfgManager inst;
        public static EquipCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new EquipCfgManager();
            }
            return inst;
        }

        private Dictionary<int, EQUIPRESOURCEEx> map = null;

        public EquipCfgManager()
        {
            map = CreateCsv<EQUIPRESOURCEEx>("equipresource");
            foreach (var item in map.Values)
            {
                item.Parse();
            }
        }

        public EQUIPRESOURCEEx GetItemById(int id)
        {
            EQUIPRESOURCEEx item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
