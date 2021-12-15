using UnityEngine;

namespace Inputs
{
    public class InputHandler : MonoBehaviour
    {
        static public InputHandler current;

#if UNITY_EDITOR
        public bool touchSimulation = true;
#endif

        private void OnEnable()
        {
            current = this;
        }
        private void OnDisable()
        {
            current = null;
        }


        Vector2 touch_pos_record;
        bool pos_used_in_frame = false;
        Vector2 touch_delta_record;
        bool delta_used_in_frame = false;

        private void Update()
        {
            pos_used_in_frame = false;
            delta_used_in_frame = false;
        }


        public bool isTouchDown()
        {
#if UNITY_EDITOR
            if (touchSimulation) return Input.GetMouseButton(0);
#endif
            return Input.touchCount > 0;
        }


        public Vector2 getTouchPosition()
        {
            if (pos_used_in_frame) return touch_pos_record;
            pos_used_in_frame = true;

#if UNITY_EDITOR
            if (touchSimulation)
            {
                touch_pos_record = Input.mousePosition;
                return touch_pos_record;
            }
#endif
            touch_pos_record = Input.touches[0].position;
            return touch_pos_record;
        }


#if UNITY_EDITOR
        Vector2 last_mouse_pos;
#endif

        public Vector2 getDelta()
        {
            if (delta_used_in_frame) return touch_delta_record;
            delta_used_in_frame = true;

#if UNITY_EDITOR
            if (touchSimulation)
            {
                touch_delta_record = (Vector2)Input.mousePosition - last_mouse_pos;
                last_mouse_pos = Input.mousePosition;
                return touch_delta_record;
            }
#endif
            touch_delta_record = Input.touches[0].deltaPosition;
            return touch_delta_record;
        }
    }
}
