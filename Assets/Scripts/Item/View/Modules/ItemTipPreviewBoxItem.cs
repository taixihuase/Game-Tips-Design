using Item.Control;
using UnityEngine.EventSystems;

namespace Item.View.Modules
{
    public class ItemTipPreviewBoxItem : ItemIcon
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            ItemTipManager.Inst().OpenTipView(itemData);
        }
    }
}
