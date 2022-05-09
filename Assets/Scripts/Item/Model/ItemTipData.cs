using Item.Enum;
using UnityEngine;

namespace Item.Model
{
    public class ItemTipData
    {
        public ItemTipType tipType;
        //tip根节点的坐标
        public Vector2 pos = Vector2.zero;
        //tip上anchor所代表的点固定处在pos坐标
        public Vector2 anchor = center;
        //tip背景拉伸时pivot所代表的点固定不动
        public Vector2 pivot = center;
        //是否显示遮罩
        public bool showMask;

        //标识是不是对比tip
        private bool _isCompare;
        public bool isCompare
        {
            get { return _isCompare; }
            set
            {
                _isCompare = value;
                if (value)
                {
                    isAdditionalPart = true;
                    canOperate = false;
                }
                else
                {
                    isCompareLeftPart = false;
                }
            }
        }

        //标识对比tip是否在左边
        private bool _isCompareLeftPart;
        public bool isCompareLeftPart
        {
            get { return _isCompareLeftPart; }
            set
            {
                _isCompareLeftPart = value;
                if (value)
                {
                    isCompare = true;
                }
            }
        }

        //标识第一个附加tip和第二个附加tip
        private int _additionalPartIndex;
        public int additionalPartIndex
        {
            get { return _additionalPartIndex; }
            set
            {
                _additionalPartIndex = value;
                if (value > 0)
                {
                    isAdditionalPart = true;
                }
                else
                {
                    isAdditionalPart = false;
                    isCompare = false;
                }
            }
        }

        //标识是否同时显示的tip
        private bool _isAdditionalPart;
        public bool isAdditionalPart
        {
            get { return _isAdditionalPart; }
            set
            {
                _isAdditionalPart = value;
                if (value)
                {
                    //附加tip不加遮罩，遮罩由原始tip决定
                    showMask = false;
                }
            }
        }

        //标识是否有操作按钮
        public bool canOperate;

        private static Vector2 center = new Vector2(0.5f, 0.5f); 
    }
}
