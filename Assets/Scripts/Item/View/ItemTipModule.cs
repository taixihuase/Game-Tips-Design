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

    //ģ���ڲ������Ű��ƫ��
    public abstract float Relayout();

    public abstract bool IsValid();
}
