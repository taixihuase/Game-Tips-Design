using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UILayout
    {
        public delegate void OnReposition();

        private static Dictionary<int, List<UILayoutItem>> itemPool = new Dictionary<int, List<UILayoutItem>>();

        private static GameObject poolRootObj;

        public IList ItemDatas { get; private set; }

        public List<UILayoutItem> Items { get; private set; }

        private OnReposition onReposition = null;

        private string itemPrefabPath = null;

        private int itemPrefabPathHash = 0;

        private Coroutine createCoroutine = null;
        private int maxCreateItemNumPerFrame = 20;

        private RectTransform rectTransform = null;

        private MonoBehaviour parent = null;

        public void SetParent(MonoBehaviour p, RectTransform t)
        {
            parent = p;
            rectTransform = t;
        }

        public void Init(string itemPath, OnReposition func = null)
        {
            itemPrefabPath = itemPath;
            itemPrefabPathHash = itemPath.GetHashCode();
            Items = new List<UILayoutItem>();

            if (!itemPool.ContainsKey(itemPrefabPathHash))
            {
                itemPool.Add(itemPrefabPathHash, new List<UILayoutItem>());
            }
            if (poolRootObj == null)
            {
                poolRootObj = new GameObject("Layout Items Pool");
                poolRootObj.SetActive(false);
            }

            onReposition = func;
            SetMaxCreateItemNumPerFrame();
        }

        public void SetOnReposition(OnReposition func)
        {
            onReposition = func;
        }


        public void Reposition()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            onReposition?.Invoke();
        }

        public void SetMaxCreateItemNumPerFrame(int num = 20)
        {
            maxCreateItemNumPerFrame = num;
        }

        public void SetData(IEnumerable[] datas)
        {
            SetData((IList)datas);
        }

        public void SetData(IList datas)
        {
            Recycle();

            ItemDatas = datas;
            var itemList = itemPool[itemPrefabPathHash];
            if (itemList.Count < datas.Count)
            {
                createCoroutine = parent.StartCoroutine(CreateItems());
            }
            else
            {
                PopItems();
            }
        }

        IEnumerator CreateItems()
        {
            var itemList = itemPool[itemPrefabPathHash];
            GameObject asset = AssetLoader.LoadAsset<GameObject>(itemPrefabPath);
            if (asset != null)
            {
                int count = ItemDatas.Count - itemList.Count;
                int i = 0;
                while(i < count)
                {
                    GameObject go = GameObject.Instantiate(asset);
                    UIUtils.SetParent(go.transform, poolRootObj.transform, true);
                    UILayoutItem item = go.GetComponent<UILayoutItem>();
                    itemList.Add(item);

                    i++;
                    if (i % maxCreateItemNumPerFrame == 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                PopItems();
            }

            yield return null;
        }

        private void PopItems()
        {
            var itemList = itemPool[itemPrefabPathHash];

            int i = itemList.Count - 1;
            for (int j = 0; j < ItemDatas.Count; j++)
            {
                UILayoutItem item = itemList[i];
                UIUtils.SetParent(item.transform, rectTransform, true);
                item.gameObject.SetActive(true);

                itemList.RemoveAt(i);
                i--;

                Items.Add(item);
                item.SetData(ItemDatas[j]);
            }
        }

        private void Recycle()
        {
            if (createCoroutine != null)
            {
                parent.StopCoroutine(createCoroutine);
                createCoroutine = null;
            }

            var itemList = itemPool[itemPrefabPathHash];
            for (int i = 0; i < Items.Count; i++)
            {
                UILayoutItem item = Items[i];
                item.Clear();
                UIUtils.SetParent(item.transform, poolRootObj.transform);
            }

            itemList.AddRange(Items);
            Items.Clear();
        }

        public void Clear()
        {
            ItemDatas = null;
            Recycle();
        }

        public void Destroy()
        {
            Clear();
            onReposition = null;
            itemPrefabPath = null;
            itemPrefabPathHash = 0;
        }
    }
}