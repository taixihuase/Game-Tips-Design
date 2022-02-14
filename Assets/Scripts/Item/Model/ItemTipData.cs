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
        public bool isCompare;
        public bool isCompareLeftPart;
        public bool canOperate;

        private static Vector2 center = new Vector2(0.5f, 0.5f); 
    }
}
