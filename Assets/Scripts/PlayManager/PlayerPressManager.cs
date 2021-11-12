using UnityEngine;


namespace PlayManagement
{
    public partial class PlayerPressManager : MonoBehaviour
    {

        bool wasPressed = false; // for checking if in previous frame the pivot was pressed

        float pressedDuration = 0;

        public float pressableRange = 4;

        private void Update() {
            InputUpdates ();
        }

        private void InputUpdates() {


            if (wasPressed) {

                if (InputGetter.isPoinerDown) {
                    // update 
                    pressedDuration += Time.deltaTime;
                    IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressDownUpdate ());

                } else {

                    wasPressed = false;
                    IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressUp (pressedDuration));
                    pressedDuration = 0;

                }

            } else {

                bool pointerInsideRange() => Vector2.Distance (InputGetter.GetPointerWorldPosition (), Vector2.zero) <= pressableRange;

                if (InputGetter.isPoinerDown && pointerInsideRange ()) {

                    wasPressed = true;
                    IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressDown (pressedDuration));
                    pressedDuration = 0;


                } else {

                    // update
                    pressedDuration += Time.deltaTime;
                    IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressUpUpdate ());
                }

            }
        }
    }
}
