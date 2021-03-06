using Core;
using Core.UI;
using Item.Enum;
using Item.View;
using Item.View.Modules;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Control
{
    public class ItemTipPool
    {
        private static ItemTipPool inst;
        public static ItemTipPool Inst()
        {
            if (inst == null)
            {
                inst = new ItemTipPool();
            }
            return inst;
        }

        private Transform poolRoot;

        private const string tipPath = "ui/window/itemtip/ui_itemtipview.prefab";
        private const int initialTipCount = 2;
        private List<ItemTipView> tipPool = new List<ItemTipView>();

        private const int initialModuleCount = 2;
        private Dictionary<int, List<ItemTipModule>> modulePool = new Dictionary<int, List<ItemTipModule>>();
        private Dictionary<int, string> modulePrefabPath = new Dictionary<int, string>();
        private ItemTipView[] usingTips = new ItemTipView[5] { null, null, null, null, null };


        public ItemTipPool()
        {
            GameObject go = new GameObject("ItemTipPoolRoot");
            poolRoot = go.transform;

            PreCreateTips();

            AddModulesPath();
            PreCreateModules();
        }

        #region Tips

        public void PushTip(ItemTipView tip)
        {
            if (tip == null)
            {
                return;
            }

            tip.transform.position = Vector3.zero;
            tip.gameObject.SetActive(false);
            tip.Release();
            tipPool.Add(tip);
            SetTipUsingState(tip, false);
        }

        public ItemTipView PopTip()
        {
            ItemTipView tip;
            if (tipPool.Count > 0)
            {
                tip = tipPool[tipPool.Count - 1];
                tipPool.RemoveAt(tipPool.Count - 1);
                tip.gameObject.SetActive(true);
                SetTipUsingState(tip, true);
                return tip;
            }

            GameObject go = AssetLoader.LoadAsset<GameObject>(tipPath);
            GameObject instance = GameObject.Instantiate(go);
            instance.SetActive(true);
            instance.transform.position = Vector3.zero;
            tip = instance.GetComponent<ItemTipView>();
            SetTipUsingState(tip, true);
            return tip;
        }

        private void PreCreateTips()
        {
            GameObject go = AssetLoader.LoadAsset<GameObject>(tipPath);
            for (int i = 0; i < initialTipCount; i++)
            {
                GameObject instance = GameObject.Instantiate(go);
                PushTip(instance.GetComponent<ItemTipView>());
            }
        }

        private void SetTipUsingState(ItemTipView tip, bool isUsing)
        {
            if (isUsing)
            {
                for (int i = 0; i < usingTips.Length; i++)
                {
                    if (usingTips[i] == null)
                    {
                        usingTips[i] = tip;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < usingTips.Length; i++)
                {
                    if (usingTips[i] == tip)
                    {
                        usingTips[i] = null;
                        break;
                    }
                }
            }
        }

        public ItemTipView GetUsingTip(int index = 0)
        {
            if (index < usingTips.Length)
            {
                return usingTips[index];
            }

            return null;
        }

        private int GetUsingTipCount()
        {
            int cnt = 0;
            for (int i = 0; i < usingTips.Length; i++)
            {
                if (usingTips[i] != null)
                {
                    cnt++;
                }
            }
            return cnt;
        }

        public bool IsTipsUsingFullUp()
        {
            int cnt = GetUsingTipCount();
            return cnt == usingTips.Length;
        }

        public void RecycleAllUsingTips()
        {
            RecycleUsingTipsFromIndex(0);
        }

        public void RecycleUsingTipsFromIndex(int idx)
        {
            for (int i = idx; i < usingTips.Length; i++)
            {
                RecycleUsingTips(i);
            }
        }

        private void RecycleUsingTips(int idx)
        {
            var tip = GetUsingTip(idx);
            PushTip(tip);
        }

        #endregion

        #region Modules

        private void AddModulesPath()
        {
            AddModulePath(ItemTipModuleType.Header, "ui/window/itemtip/ui_itemtipview_headermodule.prefab");
            AddModulePath(ItemTipModuleType.BaseInfo, "ui/window/itemtip/ui_itemtipview_baseinfomodule.prefab");
            AddModulePath(ItemTipModuleType.Demand, "ui/window/itemtip/ui_itemtipview_demandmodule.prefab");
            AddModulePath(ItemTipModuleType.Attr, "ui/window/itemtip/ui_itemtipview_attrmodule.prefab");
            AddModulePath(ItemTipModuleType.Skill, "ui/window/itemtip/ui_itemtipview_skillmodule.prefab");
            AddModulePath(ItemTipModuleType.Desc, "ui/window/itemtip/ui_itemtipview_descmodule.prefab");
            AddModulePath(ItemTipModuleType.Effect, "ui/window/itemtip/ui_itemtipview_effectmodule.prefab");
            AddModulePath(ItemTipModuleType.Price, "ui/window/itemtip/ui_itemtipview_pricemodule.prefab");
            AddModulePath(ItemTipModuleType.Item, "ui/window/itemtip/ui_itemtipview_itemmodule.prefab");
            AddModulePath(ItemTipModuleType.Button, "ui/window/itemtip/ui_itemtipview_buttonmodule.prefab");
            AddModulePath(ItemTipModuleType.RightButton, "ui/window/itemtip/ui_itemtipview_rightbuttonmodule.prefab");
            AddModulePath(ItemTipModuleType.Display, "ui/window/itemtip/ui_itemtipview_displaymodule.prefab");
        }

        private void AddModulePath(int moduleType, string path)
        {
            modulePrefabPath.Add(moduleType, path);
        }
        
        public void PushModule(int moduleType, ItemTipModule module)
        {
            UIUtils.SetParent(module.transform, poolRoot, true);
            module.transform.position = Vector3.zero;
            module.Recycle();
            List<ItemTipModule> moduleList;
            if (modulePool.TryGetValue(moduleType, out moduleList))
            {
                moduleList.Add(module);
            }
            else
            {
                moduleList = new List<ItemTipModule>();
                moduleList.Add(module);
                modulePool.Add(moduleType, moduleList);
            }
        }

        public ItemTipModule PopModule(int moduleType)
        {
            List<ItemTipModule> moduleList;
            if (modulePool.TryGetValue(moduleType, out moduleList))
            {
                if (moduleList.Count > 0)
                {
                    ItemTipModule module = moduleList[moduleList.Count - 1];
                    moduleList.RemoveAt(moduleList.Count - 1);
                    return module;
                }
            }
            ItemTipModule instance = InstantiateModule(moduleType);
            return instance;
        }

        private ItemTipModule InstantiateModule(int moduleType)
        {
            string path;
            if (modulePrefabPath.TryGetValue(moduleType, out path))
            {
                GameObject go = AssetLoader.LoadAsset<GameObject>(path);
                GameObject instance = GameObject.Instantiate(go);
                UIUtils.SetParent(instance.transform, poolRoot, true);
                instance.transform.position = Vector3.zero;
                return instance.GetComponent<ItemTipModule>();
            }

            return null;
        }

        private void PreCreateModules()
        {
            foreach (var kv in modulePrefabPath)
            {
                GameObject go = AssetLoader.LoadAsset<GameObject>(kv.Value);
                for (int i = 0; i < initialModuleCount; i++)
                {
                    GameObject instance = GameObject.Instantiate(go);
                    PushModule(kv.Key, instance.GetComponent<ItemTipModule>());
                }
            }
            Resources.UnloadUnusedAssets();
        }

        #endregion
    }
}
