﻿using Core;
using Item.Enum;
using Item.View;
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

        private const string tipPath = "ui/window/itemtip/ui_itemtipview";
        private const int initialTipCount = 3;
        private List<ItemTipView> tipPool = new List<ItemTipView>();

        private const int initialModuleCount = 3;
        private Dictionary<int, List<ItemTipModule>> modulePool = new Dictionary<int, List<ItemTipModule>>();
        private Dictionary<int, string> modulePrefabPath = new Dictionary<int, string>();
        private ItemTipView[] usingTips = new ItemTipView[3] { null, null, null };


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
            AssetLoader.DestroyGameObjectAsset(go, true);
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
            AssetLoader.DestroyGameObjectAsset(go, true);
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

        public ItemTipView GetUsingTip(uint index = 0)
        {
            if (index < usingTips.Length)
            {
                return usingTips[index];
            }

            return null;
        }

        public void RecycleUsingTips()
        {
            for (int i = 0; i < usingTips.Length; i++)
            {
                PushTip(usingTips[i]);
            }
        }

        #endregion

        #region Modules

        private void AddModulesPath()
        {
            AddModulePath(ItemTipModuleType.Header, "data/ui/window/itemtip/ui_itemtipview_headermodule");
            AddModulePath(ItemTipModuleType.BaseInfo, "data/ui/window/itemtip/ui_itemtipview_baseinfomodule");
            AddModulePath(ItemTipModuleType.Demand, "data/ui/window/itemtip/ui_itemtipview_demandmodule");
            AddModulePath(ItemTipModuleType.Attr, "data/ui/window/itemtip/ui_itemtipview_attrmodule");
            AddModulePath(ItemTipModuleType.Suit, "data/ui/window/itemtip/ui_itemtipview_suitmodule");
            AddModulePath(ItemTipModuleType.Skill, "data/ui/window/itemtip/ui_itemtipview_skillmodule");
            AddModulePath(ItemTipModuleType.Desc, "data/ui/window/itemtip/ui_itemtipview_descmodule");
            AddModulePath(ItemTipModuleType.Price, "data/ui/window/itemtip/ui_itemtipview_pricemodule");
            AddModulePath(ItemTipModuleType.Item, "data/ui/window/itemtip/ui_itemtipview_itemmodule");
            AddModulePath(ItemTipModuleType.Button, "data/ui/window/itemtip/ui_itemtipview_buttonmodule");
            AddModulePath(ItemTipModuleType.RightButton, "data/ui/window/itemtip/ui_itemtipview_rightbuttonmodule");
        }

        private void AddModulePath(int moduleType, string path)
        {
            modulePrefabPath.Add(moduleType, path);
        }
        
        public void PushModule(int moduleType, ItemTipModule module)
        {
            module.transform.parent = poolRoot;
            module.transform.position = Vector3.zero;
            module.gameObject.SetActive(false);
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
                instance.transform.position = Vector3.zero;
                AssetLoader.DestroyGameObjectAsset(go);
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
                AssetLoader.DestroyGameObjectAsset(go);
            }
            Resources.UnloadUnusedAssets();
        }

        #endregion
    }
}
