using Item.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Item.Enum.ItemTipModuleType;

namespace Item.View
{
    public class ItemTipView : MonoBehaviour
    {
        public bool IsActive
        {
            get; private set;
        }

        private List<ItemTipModule> tempModuleList = new List<ItemTipModule>();

        public void AddModule(ItemTipModule module)
        {
            module.SetTipParent(this);
            tempModuleList.Add(module);
        }

        public void Release()
        {
            IsActive = false;
            ReleaseModules();
        }

        private void ReleaseModules()
        {
            tempModuleList.Clear();
        }

        public void SetData(BaseItemData itemData)
        {
            IsActive = true;
            InitRelayoutStates();

            for (int i = 0; i < tempModuleList.Count; i++)
            {
                ItemTipModule module = tempModuleList[i];
                module.SetData(itemData);
            }
        }

        #region Relayout

        private enum RelayoutState
        {
            Unready = 0,
            Ready = 1,
            Finished = 2
        }

        public RectTransform topContextRoot;
        public RectTransform bottomContextRoot;
        public ScrollRect scrollRect;

        private float topRelayoutOffset = 0;
        private float middleRelayoutOffset = 0;
        private float bottomRelayoutOffset = 0;
        private int currentRelayoutIndex = 0;

        private List<RelayoutState> relayoutStates;
        private Vector3 tempVec3 = new Vector3(0, 0, 0);

        private void InitRelayoutStates()
        {
            relayoutStates = new List<RelayoutState>(tempModuleList.Count);
            topRelayoutOffset = 0;
            middleRelayoutOffset = 0;
            bottomRelayoutOffset = 0;
            currentRelayoutIndex = 0;
        }

        public void CallRelayout(ItemTipModule module)
        {
            if (IsActive && tempModuleList.Count > currentRelayoutIndex)
            {
                ItemTipModule currentModule = tempModuleList[currentRelayoutIndex];
                if (currentModule == module)
                {
                    Relayout();
                }
            }
        }    

        private void Relayout()
        {
            if (RelayoutLayer(ModuleLayerType.Top))
            {
                scrollRect.transform.localPosition = tempVec3;
                if (RelayoutLayer(ModuleLayerType.Scroll))
                {
                    if (RelayoutLayer(ModuleLayerType.Bottom))
                    {
                        AdjustSize();
                    }
                }
            }
        }

        private bool RelayoutLayer(ModuleLayerType layer)
        {
            int lastModuleType = 0;
            int lastModuleSubType = 0;
            float spacing;
            float moduleSize;
            Transform root;
            float offset;

            if (layer == ModuleLayerType.Top)
            {
                tempVec3.y = -topRelayoutOffset;
                offset = topRelayoutOffset;
                root = topContextRoot;
            }
            else if (layer == ModuleLayerType.Scroll)
            {
                tempVec3.y = -middleRelayoutOffset;
                offset = middleRelayoutOffset;
                root = scrollRect.content;
            }
            else
            {
                tempVec3.y = -bottomRelayoutOffset;
                offset = bottomRelayoutOffset;
                root = bottomContextRoot;
            }

            for (int i = 0; i < tempModuleList.Count; i++)
            {
                ItemTipModule module = tempModuleList[i];
                if (IsModuleInLayer(module.moduleType, layer))
                {
                    if (relayoutStates[i] == RelayoutState.Unready)
                    {
                        return false;
                    }
                    else if (relayoutStates[i] == RelayoutState.Ready)
                    {
                        if (module.IsValid())
                        {
                            module.transform.parent = root;

                            spacing = module.GetModuleSpacing(lastModuleType, lastModuleSubType);
                            tempVec3.y -= spacing;
                            module.transform.localPosition = tempVec3;
                            moduleSize = module.Relayout();
                            tempVec3.y -= moduleSize;

                            offset += moduleSize + spacing;
                            SetLayerOffset(offset, layer);

                            lastModuleType = module.moduleType;
                            lastModuleSubType = module.subModuleType;
                            relayoutStates[i] = RelayoutState.Finished;

                            currentRelayoutIndex++;
                        }
                    }
                    else
                    {
                        lastModuleType = module.moduleType;
                        lastModuleSubType = module.subModuleType;
                    }
                }
            }


            return true;

            void SetLayerOffset(float offset, ModuleLayerType layer)
            {
                if (layer == ModuleLayerType.Top)
                {
                    topRelayoutOffset = offset;
                }
                else if (layer == ModuleLayerType.Scroll)
                {
                    middleRelayoutOffset = offset;
                }
                else
                {
                    bottomRelayoutOffset = offset;
                }
            }
        }

        private void AdjustSize()
        {
            
        }

        #endregion
    }
}
