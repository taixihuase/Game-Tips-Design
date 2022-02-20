using UnityEngine;

namespace Core.UI
{
    public abstract class UILayoutItem : MonoBehaviour
    {
        public abstract void SetData(object data);

        public virtual void Clear() { }
    }
}
