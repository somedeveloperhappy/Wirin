using Gameplay;
using Statics;
using UnityEditor;
using UnityEngine;

namespace PlayManager
{
    public class PlayerPressManager : MonoBehaviour
    {

        /// <summary>
        ///     if false, it won't recieve any inputs
        /// </summary>
        public bool canGetInputs = true;

        public float pressableRange = 4;

        /// <summary>
        ///     the duration of the current state ( press, release)
        /// </summary>
        public float stateDuration;

        private bool wasPressed; // for checking if in previous frame the pivot was pressed

        private void Update()
        {
            InputUpdates();
        }

        private void InputUpdates()
        {

            if (wasPressed)
            {
                if (InputGetter.isPoinerDown && canGetInputs)
                {
                    // on down update 
                    downUpdate();
                }
                else
                {
                    // on up start
                    upStart();
                    upUpdate();
                }

            }
            else
            {

                bool pointerInsideRange()
                {
                    return Vector2.Distance(InputGetter.GetPointerWorldPosition(), Vector2.zero) <= pressableRange;
                }

                if (InputGetter.isPoinerDown && pointerInsideRange() && canGetInputs)
                {
                    // on down start
                    downStart();
                    downUpdate();
                }
                else
                {
                    // on up update
                    upUpdate();
                }
            }
        }

        private void downUpdate()
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressDownUpdate());
        }

        private void upStart()
        {
            wasPressed = false;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressUp(stateDuration));
            stateDuration = 0;
        }

        private void downStart()
        {
            wasPressed = true;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressDown(stateDuration));
            stateDuration = 0;
        }

        private void upUpdate()
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressUpUpdate());
        }

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            Handles.color = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, pressableRange);
        }
#endif
    }
}