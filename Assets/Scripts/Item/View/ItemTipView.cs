using Item.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
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
        private RectTransform topContentRoot;
        [SerializeField]
        private RectTransform bottomContentRoot;
        [SerializeField]
        private ScrollRect scrollRect;
        [SerializeField]
        private RectTransform scrollRectTrans;
        [SerializeField]
        private RectTransform rightContentRoot;

        private float topRelayoutOffset = 0;
        private float middleRelayoutOffset = 0;
        private float bottomRelayoutOffset = 0;
        private float rightRelayoutOffset = 0;
        private int currentRelayoutIndex = 0;

        private const float scrollRectTopSpacing = 4;
        private const float scrollRectBottomSpacing = 4;
        private const float maxBackgroundHeight = 500;

        private List<RelayoutState> relayoutStates = new List<RelayoutState>(16);
        private Vector3 tempVec3 = new Vector3(0, 0, 0);
        private Vector2 anchorVec2 = new Vector2(0, 0);

        private void InitRelayoutStates()
        {
            topRelayoutOffset = 0;
            middleRelayoutOffset = 0;
            bottomRelayoutOffset = 0;
            rightRelayoutOffset = 0;
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
                scrollRect.content.anchoredPosition = Vector2.zero;
                if (RelayoutLayer(ModuleLayerType.Scroll))
                {
                    scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                    var bounds = scrollRect.content.CalculateRelativeBounds();
                    scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bounds.size.y);
                    if (RelayoutLayer(ModuleLayerType.Bottom))
                    {
                        AdjustSize();
                        AdjustPos();

                        RelayoutLayer(ModuleLayerType.Right);
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
            ref float offset = ref topRelayoutOffset;
            float sign = layer == ModuleLayerType.Right ? 1 : -1;

            tempVec3.x = 0;
            if (layer == ModuleLayerType.Top)
            {
                tempVec3.y = sign * topRelayoutOffset;
                anchorVec2.y = 1;
                offset = ref topRelayoutOffset;
                root = topContentRoot;
            }
            else if (layer == ModuleLayerType.Scroll)
            {
                tempVec3.y = sign * middleRelayoutOffset;
                anchorVec2.y = 1;
                offset = ref middleRelayoutOffset;
                root = scrollRect.content;
            }
            else if (layer == ModuleLayerType.Bottom)
            {
                tempVec3.y = sign * bottomRelayoutOffset;
                anchorVec2.y = 1;
                offset = ref bottomRelayoutOffset ;
                root = bottomContentRoot;
            }
            else
            {
                tempVec3.y = sign * rightRelayoutOffset;
                anchorVec2.y = 0;
                offset = ref rightRelayoutOffset;
                root = rightContentRoot;
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
                            tempVec3.y += sign * spacing;
                            module.rectTransform.anchorMin = anchorVec2;
                            module.rectTransform.anchorMax = anchorVec2;
                            module.rectTransform.pivot = anchorVec2;
                            module.transform.localPosition = tempVec3;
                            moduleSize = module.Relayout();
                            tempVec3.y += sign * moduleSize;

                            offset += moduleSize + spacing;

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
        }

        private void AdjustSize()
        {
            float totalSize = topRelayoutOffset + scrollRectTopSpacing + middleRelayoutOffset + scrollRectBottomSpacing + bottomRelayoutOffset;
            if (totalSize <= maxBackgroundHeight)
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalSize);

                tempVec3.y = -totalSize + bottomRelayoutOffset;
                bottomContentRoot.transform.localPosition = tempVec3;

                scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, middleRelayoutOffset);
            }
            else
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxBackgroundHeight);

                tempVec3.y = -maxBackgroundHeight + bottomRelayoutOffset;
                bottomContentRoot.transform.localPosition = tempVec3;

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