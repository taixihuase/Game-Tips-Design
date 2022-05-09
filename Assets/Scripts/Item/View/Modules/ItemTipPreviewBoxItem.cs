using Item.Model;

namespace Item.View.Modules
{
    public class ItemTipPreviewBoxItem : ItemIcon
    {
        protected override (BaseItemData, BaseItemData, BaseItemData) GenerateDatas(BaseItemData itemData)
        {
            return (itemData, null, null);
        }
    }
}
