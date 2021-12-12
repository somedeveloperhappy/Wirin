using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UI
{
	public class SwipeButton : MonoBehaviour
	{
		public GraphicRaycaster graphicRaycaster;
		public float swipeThreshold = 100, resetTime = 0.75f;
		[Range (0f, 360f)] public float directionInDegree = 0; // starts from right, counter-clockwise till 360
		public UnityEngine.Events.UnityEvent OnSwipe;


		Vector2 normalizedDirection;
		Vector2 m_init_pos;
		bool was_on_gameobject = false;
		int m_fingerID = -1;
		float m_startTime = 0;

		private void Awake()
		{
			directionInDegree *= Mathf.Deg2Rad;
			normalizedDirection = new Vector2 (Mathf.Cos (directionInDegree), Mathf.Sin (directionInDegree));
			directionInDegree *= Mathf.Rad2Deg;
		}

		private void Update()
		{
			if (isPointerDown ())
			{
				if (was_on_gameobject)
				{
					OnPointerOver ();
				}
				if (EventSystem.current.IsPointerOverGameObject ())
				{
					was_on_gameobject = true;
					OnPointerOver ();
					OnPointerOver ();
				}
			}
			else
			{
				if (was_on_gameobject)
				{
					OnPointerExit ();
				}
			}
		}
        
        private bool isPointerDown()
        {
            if(Input.GetMouseButton(0))
            {
                m_fingerID = -1;
                return true;
            }
            
            foreach (var t in Input.touches)
            {
                if(isPointOnGameObject(t.position))
                {
                    m_fingerID = t.fingerId;
                    return true;
                }
            }
            
            m_fingerID = -1;
            return false;
        }
        private bool isPointerOnGameobject()
        {
            if(Input.GetMouseButton(0))
            {
                m_fingerID = -1;
                return true;
            }
            
            foreach (var t in Input.touches)
            {
                if(isPointOnGameObject(t.position))
                {
                    m_fingerID = t.fingerId;
                    return true;
                }
            }
            
            m_fingerID = -1;
            return false;
        }

		private void OnPoiterEnter()
		{
			m_init_pos = GetInputPosition ();
			m_startTime = Time.timeSinceLevelLoad;
		}

		private void OnPointerExit()
		{
			Vector2 input_pos = GetInputPosition ();
			if (hasPassedThreshold (input_pos))
			{
				OnSwipe?.Invoke ();
			}
		}

		void OnPointerOver()
		{
			float time = Time.timeSinceLevelLoad;
			Vector2 input_pos = GetInputPosition ();

			if (time - m_startTime > resetTime)
			{
				m_startTime = time;
				m_init_pos = input_pos;
				Debug.Log ($"reset");
			}
			Debug.Log ($"{Vector2.Dot (input_pos - m_init_pos, normalizedDirection)}");
		}

		private bool hasPassedThreshold(Vector2 input_pos)
		{
			return Vector2.Dot (input_pos - m_init_pos, normalizedDirection) >= swipeThreshold;
		}

		private Vector2 GetInputPosition()
		{
#if UNITY_EDITOR
			if (Input.GetMouseButton (0))
			{
				m_fingerID = -1;
				return Input.mousePosition;
			}
#endif
			foreach (var t in Input.touches)
			{
				if (m_fingerID == -1)
				{
					if (isPointOnGameObject (t.position)) return t.position;
				}
				else if (m_fingerID == t.fingerId)
				{
					return t.position;
				}
			}
			return Vector2.zero;
		}

		PointerEventData m_pointerEventData;
		List<RaycastResult> results = new List<RaycastResult> ();

		private bool isPointOnGameObject(Vector2 point)
		{
			m_pointerEventData ??= new PointerEventData (EventSystem.current);
			m_pointerEventData.position = point;
			graphicRaycaster.Raycast (m_pointerEventData, results);
			return results.Count > 0 && results[0].gameObject == gameObject;
		}


	}
}
