using System.ComponentModel;

namespace Item.Enum
{
    public enum ItemTipType
    {
        [Description("NORMAL")]
        Item = 1,

        [Description("EQUIP")]
        Equip = Item + 1,

        [Description("MOUNT")]
        Mount = Equip + 1,

        [Description("SKILLBOOK")]
        Skill = Mount + 1,

        [Description("TREASUREBOX")]
        Box = Skill + 1,

        [Description("CURRENCY")]
        Currency = Box + 1,
    }
}
