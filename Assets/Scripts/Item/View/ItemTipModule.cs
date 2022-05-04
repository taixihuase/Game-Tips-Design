using Item.Model;
using Item.View;
using UnityEngine;
using Core.UI;

public abstract class ItemTipModule : MonoBehaviour
{
    public virtual int moduleType
    {
        get
        {
            return 0;
        }
    }

    public int subModuleType
    {
        get;set;
    }

    private ItemTipView parentView;

    public BaseItemData itemData
    {
        get; private set;
    }

    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    public void SetTipParent(ItemTipView view)
    {
        parentView = view;
    }

    public virtual void SetData(BaseItemData itemData)
    {
        this.itemData = itemData;
    }

    protected void OnSetDataFinished()
    {
        parentView.CallRelayout(this);
    }

    public virtual float GetModuleSpacing(int lastModuleType, int lastModuleSubType)
    {
        if (lastModuleSubType == 0)
        {
            return 0;
        }

        return 4.0f;
    }

    //ģ���ڲ������Ű��ƫ��
    public virtual float Relayout()
    {
        var bound = rectTransform.CalculateWorldBounds();
        return bound.size.y;
    }

    public abstract bool IsValid();
}
