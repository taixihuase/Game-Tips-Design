using UnityEngine;

namespace Core.UI
{
    public class UIUtils
    {
        public static void SetParent(Transform child, Transform parent, bool reset = false)
        {
            child.parent = parent;
            if (reset)
            {
                child.localPosition = Vector3.zero;
                child.localRotation = Quaternion.identity;
                child.localScale = Vector3.one;
            }
        }
    }
}
