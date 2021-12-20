using CanvasSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class SwipeButton : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled, IPointerDownHandler, IPointerUpHandler
    {
        public GraphicRaycaster graphicRaycaster;
        public float swipeThreshold = 100, resetTime = 0.75f;
        [Range( 0f, 360f )] public float directionInDegree = 270; // starts from right, counter-clockwise till 360
        public UnityEngine.Events.UnityEvent OnSwipe;


        Vector2 normalizedDirection;
        Vector2 m_init_pos;
        bool was_on_gameobject = false;
        float m_startTime = 0;
        Vector2 last_pointer_pos;

        bool pointer_over_object = false;

        private void Awake()
        {
            directionInDegree *= Mathf.Deg2Rad;
            normalizedDirection = new Vector2( Mathf.Cos( directionInDegree ), Mathf.Sin( directionInDegree ) );
            directionInDegree *= Mathf.Rad2Deg;
        }

        private void Update()
        {
            if (isPointerDown())
            {
                if (was_on_gameobject)
                {
                    OnSwipeUpdate();
                }
                else if (pointer_over_object)
                {
                    was_on_gameobject = true;
                    OnSwipeStart();
                    OnSwipeUpdate();
                }
                last_pointer_pos = GetInputPosition();
            }
            else
            {
                if (was_on_gameobject)
                {
                    was_on_gameobject = false;
                    OnSwipeEnd();
                }
            }
        }
        public void OnPointerUp(PointerEventData eventData) => pointer_over_object = false;
        public void OnPointerDown(PointerEventData eventData) => pointer_over_object = true;

        private bool isPointerDown() => Inputs.InputHandler.current.isTouchDown();

        private void OnSwipeStart()
        {
            //Debug.Log($"on swipe start");
            m_init_pos = GetInputPosition();
            m_startTime = Time.realtimeSinceStartup;
        }

        private void OnSwipeEnd()
        {
            //Debug.Log($"on swipe end");
            if (hasPassedThreshold( last_pointer_pos ))
            {
                OnSwipe?.Invoke();
            }
        }

        void OnSwipeUpdate()
        {
            //Debug.Log($"on swipe update");
            float time = Time.realtimeSinceStartup;
            Vector2 input_pos = GetInputPosition();

            if (time - m_startTime > resetTime)
            {
                m_startTime = time;
                m_init_pos = input_pos;
                //Debug.Log($"reset");
            }
        }

        private bool hasPassedThreshold(Vector2 input_pos)
        {
            return GetSwipeAmount( input_pos ) >= swipeThreshold;
        }

        private float GetSwipeAmount(Vector2 input_pos)
        {
            return Vector2.Dot( input_pos - m_init_pos, normalizedDirection );
        }

        private Vector2 GetInputPosition() => Inputs.InputHandler.current.getTouchPosition();

        PointerEventData m_pointerEventData;
        List<RaycastResult> results = new List<RaycastResult>();

        private bool isPointOnGameObject(Vector2 point)
        {
            m_pointerEventData ??= new PointerEventData( EventSystem.current );
            m_pointerEventData.position = point;
            graphicRaycaster.Raycast( m_pointerEventData, results );
            return results.Count > 0 && results[0].gameObject == gameObject;
        }

        void IOnCanvasEnabled.OnCanvasEnable() => this.enabled = true;
        void IOnCanvasDisabled.OnCanvasDisable() => this.enabled = false;


    }
}
