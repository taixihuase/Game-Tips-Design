using System.Collections;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIGrid : GridLayoutGroup
    {
        private UILayout layout;
        protected UILayout Layout
        {
            get
            {
                if (layout == null)
                {
                    layout = new UILayout();
                    layout.SetParent(this, rectTransform);
                }
                return layout;
            }
        }

        public void Init(string itemPath, UILayout.OnReposition onReposition)
        {
            Layout.Init(itemPath, onReposition);
        }

        public void SetOnReposition(UILayout.OnReposition onReposition)
        {
            Layout.SetOnReposition(onReposition);
        }

        public void Reposition()
        {
            Layout.Reposition();
        }

        public void SetData(IEnumerable[] datas)
        {
            Layout.SetData(datas);
        }

        public void SetData(IList datas)
        {
            Layout.SetData(datas);
        }

        public void Clear()
        {
            Layout.Clear();
        }

        public void Destroy()
        {
            Layout.Destroy();
            layout = null;
        }
    }
}