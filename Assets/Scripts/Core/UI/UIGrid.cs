using System.Collections;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIGrid : GridLayoutGroup
    {
        private UILayout layout;

        protected override void Awake()
        {
            layout = new UILayout();
            layout.SetParent(this, rectTransform);
        }

        public void Init(string itemPath, UILayout.OnReposition onReposition)
        {
            layout.Init(itemPath, onReposition);
        }

        public void SetOnReposition(UILayout.OnReposition onReposition)
        {
            layout.SetOnReposition(onReposition);
        }

        public void SetMaxCreateItemNumPerFrame(int num)
        {
            layout.SetMaxCreateItemNumPerFrame(num);
        }

        public void Reposition()
        {
            layout.Reposition();
        }

        public void ForceRebuildLayoutImmediate()
        {
            layout.ForceRebuildLayoutImmediate();
        }

        public void CallOnReposition()
        {
            layout.CallOnReposition();
        }

        public void SetData(IEnumerable[] datas)
        {
            layout.SetData(datas);
        }

        public void SetData(IList datas)
        {
            layout.SetData(datas);
        }

        public IList GetData()
        {
            return layout.ItemDatas;
        }

        public IList GetItems()
        {
            return layout.Items;
        }

        public void Clear()
        {
            layout.Clear();
        }

        public void Destroy()
        {
            layout.Destroy();
            layout = null;
        }
    }
}