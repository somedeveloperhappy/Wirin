using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Player
{
    public class Shield : MonoBehaviour
    {
        public const string shieldTag = "shield";

        #region refs
        public Transform shieldTrans;
        #endregion

        #region events
        public event Action onShieldUp;
        public event Action onShieldDown;
        public event Action onDuringShieldUp;
        public event Action onDuringShieldDown;
        #endregion

        [System.Serializable]
        public class ShieldSettings
        {
            public float shieldRotateSpeed = 10;
            public float shieldUpLerpSpeed = 10;
            public float shieldDownLerpSpeed = 10;
        }
        public ShieldSettings shieldSettings;


        private Vector2 start_pos;


        #region handy refs
        Gameplay.Player.Trinon trinon => References.playerInfo.parts.mainTrinon;
        Gameplay.Player.Pivot pivot => References.playerInfo.parts.pivot;
        #endregion

        private void OnEnable() => shieldTrans.gameObject.SetActive(false);

        public void ShieldUp(PointerEventData pointerEventData)
        {
            start_pos = pointerEventData.position;

            shieldTrans.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(ShieldUpAsync());

            SetShieldRotation();
            onShieldUp?.Invoke();
        }
        public void ShildUpdate(PointerEventData pointerEventData)
        {
            SetShieldRotation();
        }

        public void ShieldDown()
        {
            shieldTrans.gameObject.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(ShieldDownAsync());
            onShieldDown?.Invoke();
        }

        private void SetShieldRotation()
        {
            // set pivot rotation
            Vector2 delta_pos = Inputs.InputHandler.current.getTouchPosition() - start_pos;
            pivot.transform.up = Vector3.Lerp(shieldTrans.up, delta_pos, shieldSettings.shieldRotateSpeed * Time.deltaTime);
        }

        IEnumerator ShieldUpAsync()
        {
            shieldTrans.localScale = Vector3.zero;

            do
            {
                shieldTrans.localScale += Vector3.one * shieldSettings.shieldUpLerpSpeed * Time.deltaTime;
                onDuringShieldUp?.Invoke();
                yield return null;
            } while (shieldTrans.localScale.x < 1);

            // absolute
            shieldTrans.localScale = Vector3.one;
        }
        IEnumerator ShieldDownAsync()
        {
            do
            {
                shieldTrans.localScale -= Vector3.one * shieldSettings.shieldDownLerpSpeed * Time.deltaTime;
                yield return null;
                onDuringShieldDown?.Invoke();
            } while (shieldTrans.localScale.x > 0);

            // absolute
            shieldTrans.localScale = Vector3.zero;
        }
    }
}