using UnityEngine;

namespace Gameplay.Player
{
    public class Pivot : MonoBehaviour, IOnPlayerPress
    {

        #region refs

        public PlayerInfo playerInfo;

        #endregion

        private float scale_curve_t;

        /// <summary>
        ///     the time for last key in scaleOnPress
        /// </summary>
        private float scale_onpress_curve_last_time;

        public AnimationCurve scaleOnPress;
        public float speedOnDown = 1, speedOnUp = 10;

        public void OnPressDown(float duration) { }

        public void OnPressUp(float duration) { }

        public void OnPressDownUpdate()
        {
            scale_curve_t += Time.deltaTime * speedOnDown;
            ApplyScale();
        }


        public void OnPressUpUpdate()
        {
            scale_curve_t -= Time.deltaTime * speedOnUp;
            ApplyScale();
        }

        public PlayerInfo GetPlayerInfo()
        {
            return playerInfo;
        }

        private void OnEnable() => this.OnPlayerPressInit();
        private void OnDisable() => this.OnPlayerPressDestroy();

        protected virtual void Start()
        {

            scale_onpress_curve_last_time = scaleOnPress.keys[scaleOnPress.keys.Length - 1].time;
        }

        private void ApplyScale()
        {
            scale_curve_t = Mathf.Clamp(scale_curve_t, 0, scale_onpress_curve_last_time);
            transform.localScale = Vector3.one * scaleOnPress.Evaluate(scale_curve_t);
        }
    }
}