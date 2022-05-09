using Item.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.UI;
using static Item.Enum.ItemTipModuleType;
using Item.Control;
using Item.View.Modules;

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
            for (int i = 0; i < tempModuleList.Count; i++)
            {
                ItemTipModule module = tempModuleList[i];
                ItemTipPool.Inst().PushModule(module.moduleType, module);
            }

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
        private RectTransform root;
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
        private const float compareTipSpacingX = 2;

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
            for (int i = 0; i < tempModuleList.Count; i++)
            {
                if (tempModuleList[i] == module)
                {
                    relayoutStates[i] = RelayoutState.Ready;
                    break;
                }
            }

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
            if (RelayoutLayer(ModuleLayerType.Top) >= 0)
            {
                tempVec3.y -= scrollRectTopSpacing;
                scrollRect.transform.localPosition = tempVec3;
                scrollRect.content.anchoredPosition = Vector2.zero;

                int middleLayerModuleCnt;
                if ((middleLayerModuleCnt = RelayoutLayer(ModuleLayerType.Scroll)) >= 0)
                {
                    scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);
                    var bounds = scrollRect.content.CalculateWorldBounds();
                    scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bounds.size.y);

                    int bottomLayerModuleCnt;
                    if ((bottomLayerModuleCnt = RelayoutLayer(ModuleLayerType.Bottom)) >= 0)
                    {
                        AdjustSize(middleLayerModuleCnt, bottomLayerModuleCnt);
                        AdjustPos();

                        RelayoutLayer(ModuleLayerType.Right);
                    }
                }
            }
        }

        private int RelayoutLayer(ModuleLayerType layer)
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

            int finishedModuleCount = 0;
            for (int i = 0; i < tempModuleList.Count; i++)
            {
                ItemTipModule module = tempModuleList[i];
                if (IsModuleInLayer(module.moduleType, layer))
                {
                    if (relayoutStates[i] == RelayoutState.Unready)
                    {
                        //��ǰ����δˢ����ϣ���ͳ�Ƹò�ģ������
                        return -1;
                    }
                    else if (relayoutStates[i] == RelayoutState.Ready)
                    {
                        if (module.IsValid)
                        {
                            module.gameObject.SetActive(true);
                            UIUtils.SetParent(module.transform, root, true);

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
                        }
                        relayoutStates[i] = RelayoutState.Finished;
                        currentRelayoutIndex++;
                        finishedModuleCount++;
                    }
                    else
                    {
                        lastModuleType = module.moduleType;
                        lastModuleSubType = module.subModuleType;
                        finishedModuleCount++;
                    }
                }
            }


            return finishedModuleCount;
        }

        private ItemTipModule FindFirstValidModuleWithinRange(int start, int end)
        {
            ItemTipModule module;
            int sign = start < end ? 1 : -1;
            for (int i = start; sign > 0 ? i < end : i >= end; i += sign)
            {
                module = tempModuleList[i];
                if (module != null && module.IsValid)
                {
                    return module;
                }
            }
            return null;
        }

        private void AdjustSize(int middleCnt, int bottomCnt)
        {
            float totalSize = topRelayoutOffset;

            ItemTipModule midModule = null;
            ItemTipModule bottomModule = null;
            if (middleCnt > 0)
            {
                //�ҵ�һ�������������Чģ��
                midModule = FindFirstValidModuleWithinRange(currentRelayoutIndex - bottomCnt - 1, currentRelayoutIndex - bottomCnt - middleCnt);
                if (midModule != null)
                {
                    totalSize += middleRelayoutOffset + scrollRectTopSpacing;
                }
            }

            float toBottomSpacing = 0;
            if (bottomCnt > 0)
            {
                //�ҵ��ײ��������һ����Чģ��
                bottomModule = FindFirstValidModuleWithinRange(currentRelayoutIndex - 1, currentRelayoutIndex - bottomCnt);
                if (bottomModule != null)
                {
                    totalSize += bottomRelayoutOffset;
                    if (midModule != null)
                    {
                        totalSize += scrollRectBottomSpacing;
                    }
                    else
                    {
                        totalSize += scrollRectTopSpacing;
                    }

                    //��ײ��Ӽ��
                    toBottomSpacing = bottomModule.GetLastModuleToBottomSpacing();
                    totalSize += toBottomSpacing;
                }
            }

            if (totalSize <= maxBackgroundHeight)
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalSize);

                //���õײ����򣨺��ײ���ಿ�֣�����tip�ױ�
                tempVec3.y = -totalSize + bottomRelayoutOffset + toBottomSpacing;
                bottomContentRoot.transform.localPosition = tempVec3;

                scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, middleRelayoutOffset);
            }
            else
            {
                background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxBackgroundHeight);

                //���õײ����򣨺��ײ���ಿ�֣�����tip�ױ�
                tempVec3.y = -maxBackgroundHeight + bottomRelayoutOffset + toBottomSpacing;
                bottomContentRoot.transform.localPosition = tempVec3;

                scrollRect.viewport.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    maxBackgroundHeight - (topRelayoutOffset + scrollRectTopSpacing + scrollRectBottomSpacing + bottomRelayoutOffset));
            }
        }

        private void AdjustPos()
        {
            ItemTipData tipData = itemData.tipData;
            root.localPosition = tipData.pos;

            background.rectTransform.pivot = tipData.pivot;
            tempVec3.x = background.rectTransform.rect.width * (tipData.pivot.x - 0.5f);
            tempVec3.y = background.rectTransform.rect.height * (tipData.pivot.y - 0.5f);

            Vector2 anchor = tipData.anchor;
            float bgWidth = background.rectTransform.rect.width;
            float bgHeight = background.rectTransform.rect.height;

            background.rectTransform.anchorMin = anchor;
            background.rectTransform.anchorMax = anchor;
            tempVec3.x += bgWidth * (0.5f - anchor.x);
            tempVec3.y += bgHeight * (0.5f - anchor.y);

            background.rectTransform.anchoredPosition = tempVec3;

            tempVec3 = root.anchoredPosition;
            if (tipData.additionalPartIndex > 0)
            {
                //��ԭʼtip������ƫ��
                tempVec3.x += tipData.additionalPartIndex * (bgWidth + compareTipSpacingX) * (tipData.isCompareLeftPart ? -1 : 1);
            }
            root.anchoredPosition = tempVec3;
        }

        #endregion
    }
}