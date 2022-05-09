using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UITable : HorizontalOrVerticalLayoutGroup
    {
        private UILayout layout;

        protected override void Awake()
        {
            layout = new UILayout();
            layout.SetParent(this, rectTransform);
        }

        public void Init(string itemPath, UILayout.OnReposition onReposition)
        {
            layout.Init(itemPath, onReposition);
        }

        public void SetOnReposition(UILayout.OnReposition onReposition)
        {
            layout.SetOnReposition(onReposition);
        }

        public void SetMaxCreateItemNumPerFrame(int num)
        {
            layout.SetMaxCreateItemNumPerFrame(num);
        }

        public void Reposition()
        {
            layout.Reposition();
        }

        public void ForceRebuildLayoutImmediate()
        {
            layout.ForceRebuildLayoutImmediate();
        }

        public void CallOnReposition()
        {
            layout.CallOnReposition();
        }

        public void SetData(IEnumerable[] datas)
        {
            layout.SetData(datas);
        }

        public void SetData(IList datas)
        {
            layout.SetData(datas);
        }

        public IList GetData()
        {
            return layout.ItemDatas;
        }

        public IList GetItems()
        {
            return layout.Items;
        }

        public void Clear()
        {
            layout.Clear();
        }

        public void Destroy()
        {
            layout.Destroy();
            layout = null;
        }

        [SerializeField] protected bool m_Vertical;
        [SerializeField] protected float m_VerticalSpacing = 0;
        [SerializeField] protected float m_HorizontalSpacing = 0;
        [SerializeField] protected int m_MaxRow = 0;
        [SerializeField] protected int m_MaxColumn = 0;
        [SerializeField] protected bool m_Tight = false;

        public bool vertical { get { return m_Vertical; } set { SetProperty(ref m_Vertical, value); } }
        public float verticalSpacing { get { return m_VerticalSpacing; } set { SetProperty(ref m_VerticalSpacing, value); } }
        public float horizontalSpacing { get { return m_HorizontalSpacing; } set { SetProperty(ref m_HorizontalSpacing, value); } }
        public int maxRow { get { return m_MaxRow; } set { SetProperty(ref m_MaxRow, value); } }
        public int maxColumn { get { return m_MaxColumn; } set { SetProperty(ref m_MaxColumn, value); } }
        public bool tight { get { return m_Tight; } set { SetProperty(ref m_Tight, value); } }

        protected Dictionary<int, float> alongOtherAxisLineTotalMinSize = new Dictionary<int, float>();
        protected Dictionary<int, float> alongOtherAxisLineTotalPreferredSize = new Dictionary<int, float>();
        protected Dictionary<int, float> alongOtherAxisLineTotalFlexibleSize = new Dictionary<int, float>();

        protected Dictionary<int, float> alongAxisLineTotalMinSize = new Dictionary<int, float>();
        protected Dictionary<int, float> alongAxisLineTotalPreferredSize = new Dictionary<int, float>();
        protected Dictionary<int, float> alongAxisLineTotalFlexibleSize = new Dictionary<int, float>();

        protected float GetAlongOtherAxisLineTotalMinSize(int line)
        {
            return alongOtherAxisLineTotalMinSize[line];
        }

        protected float GetAlongOtherAxisLineTotalPreferredSize(int line)
        {
            return alongOtherAxisLineTotalPreferredSize[line];
        }

        protected float GetAlongOtherAxisLineTotalFlexibleSize(int line)
        {
            return alongOtherAxisLineTotalFlexibleSize[line];
        }

        protected float GetAlongAxisLineTotalMinSize(int line)
        {
            return alongAxisLineTotalMinSize[line];
        }

        protected float GetAlongAxisLineTotalPreferredSize(int line)
        {
            return alongAxisLineTotalPreferredSize[line];
        }

        protected float GetAlongAxisLineTotalFlexibleSize(int line)
        {
            return alongAxisLineTotalFlexibleSize[line];
        }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            MyCalcAlongAxis(0, vertical);
        }

        public override void CalculateLayoutInputVertical()
        {
            MyCalcAlongAxis(1, vertical);
        }

        public override void SetLayoutHorizontal()
        {
            MySetChildrenAlongAxis(0, vertical);
        }

        public override void SetLayoutVertical()
        {
            MySetChildrenAlongAxis(1, vertical);
        }

        protected void MyCalcAlongAxis(int axis, bool isVertical)
        {
            int rectChildrenCount = rectChildren.Count;
            if (rectChildrenCount == 0)
                return;

            float combinedPadding = (axis == 0 ? padding.horizontal : padding.vertical);
            bool controlSize = (axis == 0 ? m_ChildControlWidth : m_ChildControlHeight);
            bool useScale = (axis == 0 ? m_ChildScaleWidth : m_ChildScaleHeight);
            bool childForceExpandSize = (axis == 0 ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);

            float totalMin = combinedPadding;
            float totalPreferred = combinedPadding;
            float totalFlexible = 0;

            bool alongOtherAxis = (isVertical ^ (axis == 1));
            float alongAxisSpacing = isVertical ? verticalSpacing : horizontalSpacing;
            float alongOtherAxisSpacing = isVertical ? horizontalSpacing : verticalSpacing;
            int maxLine = isVertical ? maxRow : maxColumn;
            maxLine = maxLine > 0 ? maxLine : rectChildrenCount;

            float tempTotalMin = combinedPadding;
            float tempTotalPreferred = combinedPadding;
            float tempTotalFlexible = 0;

            if (alongOtherAxis)
            {
                alongOtherAxisLineTotalMinSize.Clear();
                alongOtherAxisLineTotalPreferredSize.Clear();
                alongOtherAxisLineTotalFlexibleSize.Clear();

                int maxAlongOtherAxisLine = Mathf.CeilToInt(rectChildrenCount * 1.0f / maxLine);

                int iStart = m_ReverseArrangement ? maxLine - 1 : 0;
                int iEnd = m_ReverseArrangement ? 0 : maxLine;
                int iIncrement = m_ReverseArrangement ? -1 : 1;
                for (int i = iStart; m_ReverseArrangement ? i >= iEnd : i < iEnd; i += iIncrement)
                {
                    tempTotalMin = combinedPadding;
                    tempTotalPreferred = combinedPadding;
                    tempTotalFlexible = 0;

                    int jStart = m_ReverseArrangement ? maxAlongOtherAxisLine - 1 : 0;
                    int jEnd = m_ReverseArrangement ? 0 : maxAlongOtherAxisLine;
                    int jIncrement = m_ReverseArrangement ? -1 : 1;
                    for (int j = jStart; m_ReverseArrangement ? j >= jEnd : j < jEnd; j += jIncrement)
                    {
                        int idx = i + j * maxLine;
                        if (idx < rectChildrenCount)
                        {
                            RectTransform child = rectChildren[idx];
                            float min, preferred, flexible;
                            GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                            if (useScale)
                            {
                                float scaleFactor = child.localScale[axis];
                                min *= scaleFactor;
                                preferred *= scaleFactor;
                                flexible *= scaleFactor;
                            }

                            tempTotalMin += min + alongOtherAxisSpacing;
                            tempTotalPreferred += preferred + alongOtherAxisSpacing;
                            tempTotalFlexible += flexible;

                            totalMin = Mathf.Max(tempTotalMin, totalMin);
                            totalPreferred = Mathf.Max(tempTotalPreferred, totalPreferred);
                            totalFlexible = Mathf.Max(tempTotalFlexible, totalFlexible);
                        }
                    }

                    alongOtherAxisLineTotalMinSize[i] = tempTotalMin - alongOtherAxisSpacing;
                    alongOtherAxisLineTotalPreferredSize[i] = tempTotalPreferred - alongOtherAxisSpacing;
                    alongOtherAxisLineTotalFlexibleSize[i] = tempTotalFlexible;
                }

                if (rectChildrenCount > 0)
                {
                    totalMin -= alongOtherAxisSpacing;
                    totalPreferred -= alongOtherAxisSpacing;
                }
            }
            else
            {
                alongAxisLineTotalMinSize.Clear();
                alongAxisLineTotalPreferredSize.Clear();
                alongAxisLineTotalFlexibleSize.Clear();

                int iStart = m_ReverseArrangement ? rectChildrenCount - 1 : 0;
                int iEnd = m_ReverseArrangement ? 0 : rectChildrenCount;
                int iIncrement = m_ReverseArrangement ? -1 : 1;
                for (int i = iStart; m_ReverseArrangement ? i >= iEnd : i < iEnd; i += iIncrement)
                {
                    RectTransform child = rectChildren[i];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);

                    if (useScale)
                    {
                        float scaleFactor = child.localScale[axis];
                        min *= scaleFactor;
                        preferred *= scaleFactor;
                        flexible *= scaleFactor;
                    }

                    if ((i + (m_ReverseArrangement ? maxLine - iStart : maxLine)) % maxLine == 0)
                    {
                        if (i != iStart)
                        {
                            alongAxisLineTotalMinSize[m_ReverseArrangement ? Mathf.CeilToInt((i - iIncrement * 1.0f) / maxLine) : (i - iIncrement) / maxLine] = tempTotalMin - alongAxisSpacing;
                            alongAxisLineTotalPreferredSize[m_ReverseArrangement ? Mathf.CeilToInt((i - iIncrement * 1.0f) / maxLine) : (i - iIncrement) / maxLine] = tempTotalPreferred - alongAxisSpacing;
                            alongAxisLineTotalFlexibleSize[m_ReverseArrangement ? Mathf.CeilToInt((i - iIncrement * 1.0f) / maxLine) : (i - iIncrement) / maxLine] = tempTotalFlexible;
                        }

                        tempTotalMin = combinedPadding;
                        tempTotalPreferred = combinedPadding;
                        tempTotalFlexible = 0;
                    }

                    tempTotalMin += min + alongAxisSpacing;
                    tempTotalPreferred += preferred + alongAxisSpacing;
                    tempTotalFlexible += flexible;

                    totalMin = Mathf.Max(tempTotalMin, totalMin);
                    totalPreferred = Mathf.Max(tempTotalPreferred, totalPreferred);
                    totalFlexible = Mathf.Max(tempTotalFlexible, totalFlexible);
                }

                alongAxisLineTotalMinSize[m_ReverseArrangement ? 0 : (iEnd - iIncrement) / maxLine] = tempTotalMin - alongAxisSpacing;
                alongAxisLineTotalPreferredSize[m_ReverseArrangement ? 0 : (iEnd - iIncrement) / maxLine] = tempTotalPreferred - alongAxisSpacing;
                alongAxisLineTotalFlexibleSize[m_ReverseArrangement ? 0 : (iEnd - iIncrement) / maxLine] = tempTotalFlexible;

                if (rectChildrenCount > 0)
                {
                    totalMin -= alongAxisSpacing;
                    totalPreferred -= alongAxisSpacing;
                }
            }
            totalPreferred = Mathf.Max(totalMin, totalPreferred);
            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
        }

        protected void MySetChildrenAlongAxis(int axis, bool isVertical)
        {
            int rectChildrenCount = rectChildren.Count;
            if (rectChildrenCount == 0)
                return;

            float size = rectTransform.rect.size[axis];
            bool controlSize = (axis == 0 ? m_ChildControlWidth : m_ChildControlHeight);
            bool useScale = (axis == 0 ? m_ChildScaleWidth : m_ChildScaleHeight);
            bool childForceExpandSize = (axis == 0 ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);
            float alignmentOnAxis = GetAlignmentOnAxis(axis);

            bool alongOtherAxis = (isVertical ^ (axis == 1));
            float alongAxisSpacing = isVertical ? verticalSpacing : horizontalSpacing;
            float alongOtherAxisSpacing = isVertical ? horizontalSpacing : verticalSpacing;
            int maxLine = isVertical ? maxRow : maxColumn;
            maxLine = maxLine > 0 ? maxLine : rectChildrenCount;

            if (alongOtherAxis)
            {
                int maxAlongOtherAxisLine = Mathf.CeilToInt(rectChildrenCount * 1.0f / maxLine);

                int iStart = m_ReverseArrangement ? maxLine - 1 : 0;
                int iEnd = m_ReverseArrangement ? 0 : maxLine;
                int iIncrement = m_ReverseArrangement ? -1 : 1;
                for (int i = iStart; m_ReverseArrangement ? i >= iEnd : i < iEnd; i += iIncrement)
                {
                    float pos = (axis == 0 ? padding.left : padding.top);
                    float itemFlexibleMultiplier = 0;
                    float surplusSpace = size - GetAlongOtherAxisLineTotalPreferredSize(i);

                    if (surplusSpace > 0)
                    {
                        if (GetAlongOtherAxisLineTotalFlexibleSize(i) == 0)
                            pos = GetStartOffset(axis, GetAlongOtherAxisLineTotalPreferredSize(i) - (axis == 0 ? padding.horizontal : padding.vertical));
                        else if (GetAlongOtherAxisLineTotalFlexibleSize(i) > 0)
                            itemFlexibleMultiplier = surplusSpace / GetAlongOtherAxisLineTotalFlexibleSize(i);
                    }

                    float minMaxLerp = 0;
                    if (GetAlongOtherAxisLineTotalMinSize(i) != GetAlongOtherAxisLineTotalPreferredSize(i))
                        minMaxLerp = Mathf.Clamp01((size - GetAlongOtherAxisLineTotalMinSize(i)) / (GetAlongOtherAxisLineTotalPreferredSize(i) - GetAlongOtherAxisLineTotalMinSize(i)));

                    int jStart = m_ReverseArrangement ? maxAlongOtherAxisLine - 1 : 0;
                    int jEnd = m_ReverseArrangement ? 0 : maxAlongOtherAxisLine;
                    int jIncrement = m_ReverseArrangement ? -1 : 1;
                    for (int j = jStart; m_ReverseArrangement ? j >= jEnd : j < jEnd; j += jIncrement)
                    {
                        int idx = i + j * maxLine;
                        if (idx < rectChildrenCount)
                        {
                            RectTransform child = rectChildren[idx];
                            float min, preferred, flexible;
                            GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                            float scaleFactor = useScale ? child.localScale[axis] : 1f;

                            float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                            childSize += flexible * itemFlexibleMultiplier;

                            if (controlSize)
                            {
                                SetChildAlongAxisWithScale(child, axis, pos, childSize, scaleFactor);
                            }
                            else
                            {
                                float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                                SetChildAlongAxisWithScale(child, axis, pos + offsetInCell, scaleFactor);
                            }
                            pos += childSize * scaleFactor + alongOtherAxisSpacing;
                        }
                    }
                }
            }
            else
            {
                float pos = 0;
                float minMaxLerp = 0;
                float itemFlexibleMultiplier = 0;

                int iStart = m_ReverseArrangement ? rectChildrenCount - 1 : 0;
                int iEnd = m_ReverseArrangement ? 0 : rectChildrenCount;
                int iIncrement = m_ReverseArrangement ? -1 : 1;
                for (int i = iStart; m_ReverseArrangement ? i >= iEnd : i < iEnd; i += iIncrement)
                {
                    if ((i + (m_ReverseArrangement ? maxLine - iStart : maxLine)) % maxLine == 0)
                    {
                        int line = i / maxLine;
                        float totalMinSize = m_Tight ? GetAlongAxisLineTotalMinSize(line) : GetTotalMinSize(axis);
                        float totalPreferredSize = m_Tight ? GetAlongAxisLineTotalPreferredSize(line) : GetTotalPreferredSize(axis);
                        float totalFlexibleSize = m_Tight ? GetAlongAxisLineTotalFlexibleSize(line) : GetTotalFlexibleSize(axis);

                        pos = (axis == 0 ? padding.left : padding.top);
                        itemFlexibleMultiplier = 0;
                        float surplusSpace = size - totalPreferredSize;

                        if (surplusSpace > 0)
                        {
                            if (totalFlexibleSize == 0)
                                pos = GetStartOffset(axis, totalPreferredSize - (axis == 0 ? padding.horizontal : padding.vertical));
                            else if (totalFlexibleSize > 0)
                                itemFlexibleMultiplier = surplusSpace / totalFlexibleSize;
                        }

                        minMaxLerp = 0;
                        if (totalMinSize != totalPreferredSize)
                            minMaxLerp = Mathf.Clamp01((size - totalMinSize) / (totalPreferredSize - totalMinSize));
                    }

                    RectTransform child = rectChildren[i];
                    float min, preferred, flexible;
                    GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                    float scaleFactor = useScale ? child.localScale[axis] : 1f;

                    float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                    childSize += flexible * itemFlexibleMultiplier;

                    if (controlSize)
                    {
                        SetChildAlongAxisWithScale(child, axis, pos, childSize, scaleFactor);
                    }
                    else
                    {
                        float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                        SetChildAlongAxisWithScale(child, axis, pos + offsetInCell, scaleFactor);
                    }
                    pos += childSize * scaleFactor + alongAxisSpacing;
                }
            }
        }

        private void GetChildSizes(RectTransform child, int axis, bool controlSize, bool childForceExpand,
            out float min, out float preferred, out float flexible)
        {
            if (!controlSize)
            {
                min = child.sizeDelta[axis];
                preferred = min;
                flexible = 0;
            }
            else
            {
                min = LayoutUtility.GetMinSize(child, axis);
                preferred = LayoutUtility.GetPreferredSize(child, axis);
                flexible = LayoutUtility.GetFlexibleSize(child, axis);
            }

            if (childForceExpand)
                flexible = Mathf.Max(flexible, 1);
        }
    }
}
