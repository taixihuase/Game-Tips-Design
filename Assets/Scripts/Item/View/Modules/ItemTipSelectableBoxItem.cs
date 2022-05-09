using Item.Control;
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
        public Toggle SelectToggle
        {
            get
            {
                return selectToggle;
            }
        }

        public override void SetData(object data)
        {
            base.SetData(data);

            selectToggle.isOn = false;
            selectToggle.onValueChanged.AddListener(x => OnToggleChanged());
        }

        private void OnToggleChanged()
        {
            if (selectToggle.isOn)
            {
                ItemTipManager.Inst().OpenTipView(GenerateDatas(itemData));
            }
        }

        public override void Clear()
        {
            base.Clear();
            selectToggle.onValueChanged.RemoveAllListeners();
        }
    }
}
