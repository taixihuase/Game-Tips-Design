using Item.Enum;
using UnityEngine;

namespace Item.Model
{
    public class ItemTipData
    {
        public ItemTipType tipType;
        public Vector2 pos = Vector2.zero;
        //tip上anchor所代表的点固定处在pos坐标
        public Vector2 anchor = center;
        //tip背景拉伸时pivot所代表的点固定不动
        public Vector2 pivot = center;
        public bool showMask;
        public bool canCompare;

        private bool _isCompare;
        public bool isCompare
        {
            get { return _isCompare; }
            set
            {
                _isCompare = value;
                isAdditionalPart = true;
            }
        }

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

        public bool isAdditionalPart;
        public bool canOperate;

        private static Vector2 center = new Vector2(0.5f, 0.5f); 
    }
}
