using UnityEngine;


namespace PlayManagement
{
	public partial class PlayerPressManager : MonoBehaviour
	{
		bool wasPressed = false; // for checking if in previous frame the pivot was pressed

        /// <summary>
        /// the duration of the current state ( press, release)
        /// </summary>
		public float stateDuration = 0;

		public float pressableRange = 4;

		private void Update() {
			InputUpdates ();
		}

		private void InputUpdates() {
            
			if (wasPressed) {
				if (InputGetter.isPoinerDown) {
					// update 
					stateDuration += Time.deltaTime;
					IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressDownUpdate ());
				} else {
					wasPressed = false;
					IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressUp (stateDuration));
					stateDuration = 0;
				}

			} else {

				bool pointerInsideRange() => 
                    Vector2.Distance (InputGetter.GetPointerWorldPosition (), Vector2.zero) <= pressableRange;
                
				if (InputGetter.isPoinerDown && pointerInsideRange ()) {
					wasPressed = true;
					IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressDown (stateDuration));
					stateDuration = 0;
				} else {
					// update
					stateDuration += Time.deltaTime;
					IOnPlayerPressHelper.ForeachInstance ((pp) => pp.OnPressUpUpdate ());
				}
			}
		}
	}
}
