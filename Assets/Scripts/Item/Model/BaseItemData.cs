using Csv;
using CsvManager;

namespace Item.Model
{
    public class BaseItemData
    {
        public int itemId;
        public long num;
        public string currencyType;

        //后端数据
        public AbstractItem serverData;

        //Tip数据
        public ItemTipData tipData;
        public ITEMRESOURCEEx itemRes;

        public BaseItemData(AbstractItem absItem)
        {
            serverData = absItem;

            Analyze();
        }

        public BaseItemData(int id, long count)
        {
            itemId = id;
            num = count;

            Analyze();
        }

        public BaseItemData(string currency, long count)
        {
            currencyType = currency;
            num = count;

            Analyze();
        }

        public void Analyze()
        {
            if (serverData != null)
            {
                itemId = serverData.itemId;
                num = serverData.num;
                currencyType = serverData.currencyType;
            }

            if (!string.IsNullOrEmpty(currencyType))
            {
                CURRENCYRESOURCE res = CurrencyCfgManager.Inst().GetItemByType(currencyType);
                itemId = res.itemId;
            }

            if (itemId > 0)
            {
                itemRes = ItemCfgManager.Inst().GetItemById(itemId);
            }
        }
    }
}
