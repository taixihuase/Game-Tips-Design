namespace Item.Enum
{
    public class ItemTipModuleType
    {
        public const int Header = 100;
        public const int BaseInfo = Header + 100;
        public const int Demand = BaseInfo + 100;
        public const int Attr = Demand + 100;
        public const int Skill = Attr + 100;
        public const int Item = Skill + 100;
        public const int Effect = Item + 100;
        public const int Desc = Effect + 100;
        public const int Price = Desc + 100;
        public const int Button = Price + 100;
        public const int RightButton = Button + 100;
        public const int Display = RightButton + 100;

        public class AttrModuleType
        {
            public const int Base = 1;
            public const int Addition = Base + 1;
        }

        public class ItemModuleType
        {
            public const int Preview = 1;
            public const int Selectable = Preview + 1;
        }

        public enum ModuleLayerType
        {
            Top = 0,
            Scroll = 1,
            Bottom = 2,
            Right = 3,
        }

        public static bool IsModuleInLayer(int moduleType, ModuleLayerType posType)
        {
            switch (moduleType)
            {
                case Header:
                    return posType == ModuleLayerType.Top;
                case Desc:
                case Price:
                case Button:
                    return posType == ModuleLayerType.Bottom;
                case RightButton:
                    return posType == ModuleLayerType.Right;
                default:
                    return posType == ModuleLayerType.Scroll;
            }
        }
    }
}
