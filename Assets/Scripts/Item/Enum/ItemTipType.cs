namespace Item.Enum
{
    public enum ItemTipType
    {
        Item = 1,
        Equip = Item + 1,
        Mount = Equip + 1,
        Skill = Mount + 1,
        Box = Skill + 1,
        Currency = Box + 1,
    }
}
