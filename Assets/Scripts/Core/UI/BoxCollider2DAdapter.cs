using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(BoxCollider2D), typeof(RectTransform))]
    public class BoxCollider2DAdapter : MonoBehaviour
    {
        private BoxCollider2D collider2d;
        private RectTransform rt;

        [SerializeField]
        private bool alwaysUpdate = false;

        private void Start()
        {
            Adjust();
        }

        private void OnEnable()
        {
            if (!alwaysUpdate)
            {
                Adjust();
            }
        }

        private void Update()
        {
            if (alwaysUpdate)
            {
                Adjust();
            }
        }

        public void Adjust()
        {
            if (collider2d == null)
            {
                collider2d = GetComponent<BoxCollider2D>();
            }
            if (rt == null)
            {
                rt = GetComponent<RectTransform>();
            }

            if (collider2d != null && rt != null)
            {
                collider2d.offset = rt.rect.center;
                collider2d.size = new Vector2(rt.rect.width, rt.rect.height);
            }
        }
    }
}
