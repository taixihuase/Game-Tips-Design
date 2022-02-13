using Csv;
using System.Collections.Generic;

namespace CsvManager
{
    public class SkillCfgManager : AbstractCfgManager
    {
        private static SkillCfgManager inst;
        public static SkillCfgManager Inst()
        {
            if (inst == null)
            {
                inst = new SkillCfgManager();
            }
            return inst;
        }

        private Dictionary<int, SKILLRESOURCEEx> map = null;

        public SkillCfgManager()
        {
            map = CreateCsv<SKILLRESOURCEEx>("skillresource");
            foreach (var item in map.Values)
            {
                item.Parse();
            }
        }

        public SKILLRESOURCEEx GetItemById(int id)
        {
            SKILLRESOURCEEx item = null;
            map.TryGetValue(id, out item);
            return item;
        }
    }
}
