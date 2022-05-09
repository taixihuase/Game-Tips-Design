using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Item.View.Modules
{
    public class ItemTipSelectableBoxItem : ItemTipPreviewBoxItem
    {
        [SerializeField]
        private Toggle selectToggle;

        private BoxItemSelectEvent selectEvent;

        public override void SetData(object data)
        {
            base.SetData(data);

            selectToggle.isOn = false;

            if (selectEvent == null)
            {
                selectEvent = new BoxItemSelectEvent();
            }
            selectEvent.AddListener(OnBoxItemSelectChanged);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            selectToggle.isOn = true;
            selectEvent?.Invoke(this);
        }

        private void OnBoxItemSelectChanged(ItemTipSelectableBoxItem value)
        {
            if (value != this)
            {
                selectToggle.isOn = false;
            }
        }

        public override void Clear()
        {
            base.Clear();
            selectEvent?.RemoveListener(OnBoxItemSelectChanged);
        }

        private class BoxItemSelectEvent : UnityEvent<ItemTipSelectableBoxItem>
        {

        }
    }
}
