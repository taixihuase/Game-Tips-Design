using Core.UI;
using Item.Model;
using System.Collections.Generic;
using UnityEngine;

namespace Item.View
{
    public class ItemIconArea : MonoBehaviour
    {
        [SerializeField]
        private UIGrid iconGrid;

        private List<BaseItemData> iconDatas;

        private void Awake()
        {
            InitData();
        }

        private void InitData()
        {
            iconDatas = new List<BaseItemData>();

            iconDatas.Add(new BaseItemData(10001, 1));
            iconDatas.Add(new BaseItemData(10002, 1));
            iconDatas.Add(new BaseItemData(10003, 1));

            iconDatas.Add(new BaseItemData(20001, 1));
            iconDatas.Add(new BaseItemData(20002, 1));

            iconDatas.Add(new BaseItemData(30001, 1));
            iconDatas.Add(new BaseItemData(30002, 1));

            iconDatas.Add(new BaseItemData(40001, 999));
            iconDatas.Add(new BaseItemData(50001, 99));

            iconDatas.Add(new BaseItemData(60001, 1));

            iconDatas.Add(new BaseItemData("DIAMOND", 9999));
            iconDatas.Add(new BaseItemData("GOLD", 9999));
        }

        private void Start()
        {
            iconGrid.Init("ui/window/itemicon/ui_itemicon.prefab", null);
            iconGrid.SetData(iconDatas);
        }
    }
}
