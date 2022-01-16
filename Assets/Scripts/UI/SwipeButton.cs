using CanvasSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SwipeButton : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled, IDragHandler, IEndDragHandler
    {
        [Range(0f, 360f)] public float directionInDegree = 270; // starts from right, counter-clockwise till 360
        public UnityEngine.Events.UnityEvent OnSwipe;
        bool m_canDrag = true;

        public void OnCanvasDisable() => enabled = false;
        public void OnCanvasEnable() => enabled = true;

        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => m_canDrag = true;

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!m_canDrag) return;

            Vector2 norm_direction =
                transform.right * Mathf.Cos(directionInDegree * Mathf.Deg2Rad)
                + transform.up * Mathf.Sin(directionInDegree * Mathf.Deg2Rad);
            if (Vector2.Dot(eventData.delta, norm_direction) >= 1)
            {
                OnSwipe?.Invoke();
                m_canDrag = false;
            }
        }
    }
}
