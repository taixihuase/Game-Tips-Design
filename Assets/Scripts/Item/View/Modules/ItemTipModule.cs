using Item.Model;
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

        public int totalModuleType
        {
            get
            {
                return moduleType + subModuleType;
            }
        }

        protected ItemTipView parentView;

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

        private int _hideModuleTag = 0;
        public int hideModuleTag
        {
            get
            {
                return _hideModuleTag;
            }
            set
            {
                _hideModuleTag = value;
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

        protected virtual void OnSetDataFinished()
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

        //获取tip最后一个模块与tip底边的距离
        public virtual float GetLastModuleToBottomSpacing()
        {
            return 0;
        }

        //模块内部自行排版和偏移
        public virtual float Relayout()
        {
            //强制重新刷新布局大小，避免size延迟刷新问题
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
            hideModuleTag = 0;

            Clear();
        }

        protected virtual void Clear()
        {

        }
    }
}