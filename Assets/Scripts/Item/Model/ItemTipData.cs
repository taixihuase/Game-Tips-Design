using Item.Enum;
using UnityEngine;

namespace Item.Model
{
    public class ItemTipData
    {
        public ItemTipType tipType;
        public Vector2 pos = Vector2.zero;
        public Vector2 anchor = center;
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
        public bool isCompareLeftPart;
        public bool isAdditionalPart;
        public bool canOperate;

        private static Vector2 center = new Vector2(0.5f, 0.5f); 
    }
}
