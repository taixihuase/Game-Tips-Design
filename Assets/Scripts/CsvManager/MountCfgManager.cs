using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class MountCfgManager : AbstractCfgManager
    {
        private static MountCfgManager inst;
        public static MountCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new MountCfgManager();
            }
            return inst;
        }

        private Dictionary<int, MOUNTRESOURCEEx> map = null;

        public MountCfgManager()
        {
            map = CreateCsv<MOUNTRESOURCEEx>("mountresource");
            foreach (var item in map.Values)
            {
                item.Parse();
            }
        }

        public MOUNTRESOURCEEx GetItemById(int id)
        {
            MOUNTRESOURCEEx item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
