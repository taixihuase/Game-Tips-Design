using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class UITable : HorizontalOrVerticalLayoutGroup
    {
        private UILayout layout;
        protected UILayout Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new UILayout();
                    layout.SetParent(this, rectTransform);
                }
                return layout;
            }
        }

        public void Init(string itemPath, UILayout.OnReposition onReposition)
        {
            Layout.Init(itemPath, onReposition);
        }

        public void SetOnReposition(UILayout.OnReposition onReposition)
        {
            Layout.SetOnReposition(onReposition);
        }

        public void Reposition()
        {
            Layout.Reposition();
        }

        public void SetData(IEnumerable[] datas)
        {
            Layout.SetData(datas);
        }

        public void SetData(IList datas)
        {
            Layout.SetData(datas);
        }

        public void Clear()
        {
            Layout.Clear();
        }

        public void Destroy()
        {
            Layout.Destroy();
            layout = null;
        }

        [SerializeField] protected bool m_Vertical;
        [SerializeField] protected float m_VerticalSpacing = 0;
        [SerializeField] protected float m_HorizontalSpacing = 0;
        [SerializeField] protected int m_MaxRow = 0;
        [SerializeField] protected int m_MaxColumn = 0;

        public bool vertical { get { return m_Vertical; } set { SetProperty(ref m_Vertical, value); } }
        public float verticalSpacing { get { return m_VerticalSpacing; } set { SetProperty(ref m_VerticalSpacing, value); } }
        public float horizontalSpacing { get { return m_HorizontalSpacing; } set { SetProperty(ref m_HorizontalSpacing, value); } }
        public int maxRow { get { return m_MaxRow; } set { SetProperty(ref m_MaxRow, value); } }
        public int maxColumn { get { return m_MaxColumn; } set { SetProperty(ref m_MaxColumn, value); } }

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();
            MyCalcAlongAxis(0, vertical);
        }

        public override void CalculateLayoutInputVertical()
        {
            base.CalculateLayoutInputHorizontal();
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
            var rectChildrenCount = rectChildren.Count;

            if (maxLine <= 0 || maxLine >= rectChildrenCount)
            {
                for (int i = 0; i < rectChildrenCount; i++)
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

                    if (alongOtherAxis)
                    {
                        totalMin = Mathf.Max(min + combinedPadding, totalMin);
                        totalPreferred = Mathf.Max(preferred + combinedPadding, totalPreferred);
                        totalFlexible = Mathf.Max(flexible, totalFlexible);
                    }
                    else
                    {
                        totalMin += min + alongAxisSpacing;
                        totalPreferred += preferred + alongAxisSpacing;
                        totalFlexible += flexible;
                    }
                }

                if (!alongOtherAxis && rectChildrenCount > 0)
                {
                    totalMin -= alongAxisSpacing;
                    totalPreferred -= alongAxisSpacing;
                }
            }
            else
            {
                float tempTotalMin = combinedPadding;
                float tempTotalPreferred = combinedPadding;
                float tempTotalFlexible = 0;

                if (alongOtherAxis)
                {
                    int maxAlongOtherAxisLine = Mathf.CeilToInt(rectChildrenCount * 1.0f / maxLine);
                    for (int i = 0; i < maxLine; i++)
                    {
                        tempTotalMin = combinedPadding;
                        tempTotalPreferred = combinedPadding;
                        tempTotalFlexible = 0;

                        for (int j = 0; j < maxAlongOtherAxisLine; j++)
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
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (rectChildrenCount > 0)
                    {
                        totalMin -= alongOtherAxisSpacing;
                        totalPreferred -= alongOtherAxisSpacing;
                    }
                }
                else
                {
                    for (int i = 0; i < rectChildrenCount; i++)
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

                        if (i % maxLine == 0)
                        {
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

                    if (rectChildrenCount > 0)
                    {
                        totalMin -= alongAxisSpacing;
                        totalPreferred -= alongAxisSpacing;
                    }
                }
            }
            totalPreferred = Mathf.Max(totalMin, totalPreferred);
            SetLayoutInputForAxis(totalMin, totalPreferred, totalFlexible, axis);
        }

        protected void MySetChildrenAlongAxis(int axis, bool isVertical)
        {
            float size = rectTransform.rect.size[axis];
            bool controlSize = (axis == 0 ? m_ChildControlWidth : m_ChildControlHeight);
            bool useScale = (axis == 0 ? m_ChildScaleWidth : m_ChildScaleHeight);
            bool childForceExpandSize = (axis == 0 ? m_ChildForceExpandWidth : m_ChildForceExpandHeight);
            float alignmentOnAxis = GetAlignmentOnAxis(axis);

            bool alongOtherAxis = (isVertical ^ (axis == 1));
            float alongAxisSpacing = isVertical ? verticalSpacing : horizontalSpacing;
            float alongOtherAxisSpacing = isVertical ? horizontalSpacing : verticalSpacing;
            int maxLine = isVertical ? maxRow : maxColumn;
            var rectChildrenCount = rectChildren.Count;

            if (maxLine <= 0 || maxLine >= rectChildrenCount)
            {
                if (alongOtherAxis)
                {
                    float innerSize = size - (axis == 0 ? padding.horizontal : padding.vertical);

                    for(int i = 0; i < rectChildrenCount; i++)
                    {
                        RectTransform child = rectChildren[i];
                        float min, preferred, flexible;
                        GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                        float scaleFactor = useScale ? child.localScale[axis] : 1f;

                        float requiredSpace = Mathf.Clamp(innerSize, min, flexible > 0 ? size : preferred);
                        float startOffset = GetStartOffset(axis, requiredSpace * scaleFactor);
                        if (controlSize)
                        {
                            SetChildAlongAxisWithScale(child, axis, startOffset, requiredSpace, scaleFactor);
                        }
                        else
                        {
                            float offsetInCell = (requiredSpace - child.sizeDelta[axis]) * alignmentOnAxis;
                            SetChildAlongAxisWithScale(child, axis, startOffset + offsetInCell, scaleFactor);
                        }
                    }
                }
                else
                {
                    float pos = (axis == 0 ? padding.left : padding.top);
                    float itemFlexibleMultiplier = 0;
                    float surplusSpace = size - GetTotalPreferredSize(axis);

                    if (surplusSpace > 0)
                    {
                        if (GetTotalFlexibleSize(axis) == 0)
                            pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
                        else if (GetTotalFlexibleSize(axis) > 0)
                            itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
                    }

                    float minMaxLerp = 0;
                    if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
                        minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));
                
                    for (int i = 0; i < rectChildrenCount; i++)
                    {
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
            else
            {
                if (alongOtherAxis)
                {
                    float pos = (axis == 0 ? padding.left : padding.top);
                    float itemFlexibleMultiplier = 0;
                    float surplusSpace = size - GetTotalPreferredSize(axis);

                    if (surplusSpace > 0)
                    {
                        if (GetTotalFlexibleSize(axis) == 0)
                            pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.vertical : padding.horizontal));
                        else if (GetTotalFlexibleSize(axis) > 0)
                            itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
                    }

                    float minMaxLerp = 0;
                    if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
                        minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));

                    int maxAlongOtherAxisLine = Mathf.CeilToInt(rectChildrenCount * 1.0f / maxLine);
                    for (int i = 0; i < maxLine; i++)
                    {
                        float tempPos = pos;
                        for (int j = 0; j < maxAlongOtherAxisLine; j++)
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
                                    SetChildAlongAxisWithScale(child, axis, tempPos, childSize, scaleFactor);
                                }
                                else
                                {
                                    float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                                    SetChildAlongAxisWithScale(child, axis, tempPos + offsetInCell, scaleFactor);
                                }
                                tempPos += childSize * scaleFactor + alongOtherAxisSpacing;
                            }
                        }
                    }

                }
                else
                {
                    float pos = (axis == 0 ? padding.left : padding.top);
                    float itemFlexibleMultiplier = 0;
                    float surplusSpace = size - GetTotalPreferredSize(axis);

                    if (surplusSpace > 0)
                    {
                        if (GetTotalFlexibleSize(axis) == 0)
                            pos = GetStartOffset(axis, GetTotalPreferredSize(axis) - (axis == 0 ? padding.horizontal : padding.vertical));
                        else if (GetTotalFlexibleSize(axis) > 0)
                            itemFlexibleMultiplier = surplusSpace / GetTotalFlexibleSize(axis);
                    }

                    float minMaxLerp = 0;
                    float tempPos = pos;
                    if (GetTotalMinSize(axis) != GetTotalPreferredSize(axis))
                        minMaxLerp = Mathf.Clamp01((size - GetTotalMinSize(axis)) / (GetTotalPreferredSize(axis) - GetTotalMinSize(axis)));

                    for (int i = 0; i < rectChildrenCount; i++)
                    {
                        RectTransform child = rectChildren[i];
                        float min, preferred, flexible;
                        GetChildSizes(child, axis, controlSize, childForceExpandSize, out min, out preferred, out flexible);
                        float scaleFactor = useScale ? child.localScale[axis] : 1f;

                        float childSize = Mathf.Lerp(min, preferred, minMaxLerp);
                        childSize += flexible * itemFlexibleMultiplier;
                        
                        if (i % maxLine == 0)
                        {
                            tempPos = pos;
                        }
                        
                        if (controlSize)
                        {
                            SetChildAlongAxisWithScale(child, axis, tempPos, childSize, scaleFactor);
                        }
                        else
                        {
                            float offsetInCell = (childSize - child.sizeDelta[axis]) * alignmentOnAxis;
                            SetChildAlongAxisWithScale(child, axis, tempPos + offsetInCell, scaleFactor);
                        }
                        tempPos += childSize * scaleFactor + alongAxisSpacing;
                    }
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
