using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class EventTriggerListener : EventTrigger
    {
        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidDelegate onDrag;
        public VoidDelegate onDrop;
        public VoidDelegate onDeselect;
        public VoidDelegate onScroll;
        public VoidDelegate onMove;
        public VoidDelegate onInitializePotentialDrag;
        public VoidDelegate onBeginDrag;
        public VoidDelegate onEndDrag;
        public VoidDelegate onSubmit;
        public VoidDelegate onCancel;

        [SerializeField]
        public bool isThrough = false;

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick(gameObject);
            if (isThrough)
            {
                PassEvent(eventData, ExecuteEvents.submitHandler);
                PassEvent(eventData, ExecuteEvents.pointerClickHandler);
            }
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown(gameObject);
            if (isThrough)
            {
                PassEvent(eventData, ExecuteEvents.pointerDownHandler);
            }
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp(gameObject);
            if (isThrough)
            {
                PassEvent(eventData, ExecuteEvents.pointerUpHandler);
            }
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect(gameObject);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null) onDrag(gameObject);
        }

        public override void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null) onDrop(gameObject);
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            if (onDeselect != null) onDeselect(gameObject);
        }

        public override void OnScroll(PointerEventData eventData)
        {
            if (onScroll != null) onScroll(gameObject);
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (onMove != null) onMove(gameObject);
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag != null) onBeginDrag(gameObject);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (onEndDrag != null) onEndDrag(gameObject);
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            if (onSubmit != null) onSubmit(gameObject);
        }

        public override void OnCancel(BaseEventData eventData)
        {
            if (onCancel != null) onCancel(gameObject);
        }

        /// <summary>
        /// 获取或添加一个事件侦听器到指定的游戏对象。用法和NGUI一样
        /// </summary>

        public static EventTriggerListener Get(GameObject go)
        {
            EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
            if (listener == null) listener = go.AddComponent<EventTriggerListener>();
            return listener;
        }

        private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
        {
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, results);
            GameObject current = data.pointerCurrentRaycast.gameObject;
            for (int i = 0; i < results.Count; i++)
            {
                if (current != results[i].gameObject)
                {
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                }
            }
        }

    }
}
