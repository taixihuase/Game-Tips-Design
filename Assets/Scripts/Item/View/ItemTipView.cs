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

        private BaseItemData itemData;

        public void AddModule(ItemTipModule module)
        {
            module.SetTipParent(this);
            tempModuleList.Add(module);
        }

        public void Release()
        {
            IsActive = false;
            itemData = null;
            ReleaseModules();
        }

        private void ReleaseModules()
        {
            tempModuleList.Clear();
            relayoutStates.Clear();
        }

        public void SetData(BaseItemData itemData)
        {
            this.itemData = itemData;
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

        [SerializeField]
        private Image background;

        [SerializeField]
        private RectTransform topContextRoot;

        [SerializeField]
        private RectTransform bottomContextRoot;

        [SerializeField]
        private ScrollRect scrollRect;

        private float topRelayoutOffset = 0;
        private float middleRelayoutOffset = 0;
        private float bottomRelayoutOffset = 0;
        private int currentRelayoutIndex = 0;

        private const float scrollRectTopSpacing = 4;
        private const float scrollRectBottomSpacing = 4;
        private const float maxBackgroundHeight = 500;

        private List<RelayoutState> relayoutStates = new List<RelayoutState>(16);
        private Vector3 tempVec3 = new Vector3(0, 0, 0);

        private void InitRelayoutStates()
        {
            topRelayoutOffset = 0;
            middleRelayoutOffset = 0;
            bottomRelayoutOffset = 0;
            currentRelayoutIndex = 0;

            for (int i = 0; i < tempModuleList.Count; i++)
            {
                relayoutStates.Add(RelayoutState.Unready);
            }
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
                tempVec3.y -= scrollRectTopSpacing;
                scrollRect.transform.localPosition = tempVec3;
                if (RelayoutLayer(ModuleLayerType.Scroll))
                {
                    scrollRect.content.anchoredPosition = Vector2.zero;
                    if (RelayoutLayer(ModuleLayerType.Bottom))
                    {
                        AdjustSize();
                        AdjustPos();
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

            tempVec3.x = 0;
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
                            SetLayerOffset();

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

            void SetLayerOffset()
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
            float totalSize = topRelayoutOffset + scrollRectTopSpacing + middleRelayoutOffset + scrollRectBottomSpacing + bottomRelayoutOffset;
            if (totalSize <= maxBackgroundHeight)
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalSize);

                tempVec3.y = -totalSize + bottomRelayoutOffset;
                bottomContextRoot.transform.localPosition = tempVec3;

                scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, middleRelayoutOffset);
            }
            else
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxBackgroundHeight);

                tempVec3.y = -maxBackgroundHeight + bottomRelayoutOffset;
                bottomContextRoot.transform.localPosition = tempVec3;

                scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    maxBackgroundHeight - (topRelayoutOffset + scrollRectTopSpacing + scrollRectBottomSpacing + bottomRelayoutOffset));
            }
        }

        private void AdjustPos()
        {
            ItemTipData tipData = itemData.tipData;
            transform.localPosition = tipData.pos;

            background.rectTransform.pivot = tipData.pivot;
            tempVec3.x = background.rectTransform.rect.width * (tipData.pivot.x - 0.5f);
            tempVec3.y = background.rectTransform.rect.height * (tipData.pivot.y - 0.5f);

            Vector2 anchor = tipData.anchor;
            if (tipData.isCompare)
            {
                anchor.x = tipData.isCompareLeftPart ? 1 : 0;
            }
            background.rectTransform.anchorMin = anchor;
            background.rectTransform.anchorMax = anchor;
            tempVec3.x += background.rectTransform.rect.width * (0.5f - anchor.x);
            tempVec3.y += background.rectTransform.rect.height * (0.5f - anchor.y);

            background.rectTransform.anchoredPosition = tempVec3;
        }

        #endregion
    }
}