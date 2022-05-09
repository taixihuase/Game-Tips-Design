using Item.Model;
using Item.View;
using UnityEngine;
using Core.UI;
using UnityEngine.UI;


namespace Item.View.Modules
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public abstract class ItemTipModule : MonoBehaviour
    {
        public abstract int moduleType
        {
            get;
        }

        public int subModuleType
        {
            get; set;
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
            parentView?.CallRelayout(this);
        }

        public virtual float GetModuleSpacing(int lastModuleType, int lastModuleSubType)
        {
            if (lastModuleType == 0)
            {
                return 0;
            }

            return 4.0f;
        }

        //��ȡtip���һ��ģ����tip�ױߵľ���
        public virtual float GetLastModuleToBottomSpacing()
        {
            return 0;
        }

        //ģ���ڲ������Ű��ƫ��
        public virtual float Relayout()
        {
            //ǿ������ˢ�²��ִ�С������size�ӳ�ˢ������
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            var bound = rectTransform.CalculateWorldBounds();
            return bound.size.y;
        }

        public virtual bool IsValid
        {
            get; protected set;
        }

        public void Recycle()
        {
            IsValid = false;
            itemData = null;
            parentView = null;

            Clear();
        }

        protected virtual void Clear()
        {

        }
    }
}