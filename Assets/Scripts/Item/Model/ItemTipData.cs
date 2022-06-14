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
                    canOperate = false;
                }
                else
                {
                    isAdditionalLeftPart = false;
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
                }
            }
        }

        //标识附加tip是否在左边
        private bool _isAdditionalLeftPart;
        public bool isAdditionalLeftPart
        {
            get { return _isAdditionalLeftPart; }
            set
            {
                _isAdditionalLeftPart = value;
            }
        }

        //标识是否有操作按钮
        public bool canOperate;

        //标识默认隐藏的页签，第一个数字为后续模块的默认hideTag
        public int[] hideTags = DefaultHideTags1;

        private static Vector2 center = new Vector2(0.5f, 0.5f);

        public static int[] DefaultHideTags1 = new int[] { 2, ItemTipModuleType.Display };
    }
}
