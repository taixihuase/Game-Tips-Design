using Item.Model;
using Item.View;
using UnityEngine;

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

    public abstract void SetData(BaseItemData itemData);

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

    //模块内部自行排版和偏移
    public abstract float Relayout();

    public abstract bool IsValid();
}
