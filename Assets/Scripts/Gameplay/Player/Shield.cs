using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.Player
{
    public class Shield : MonoBehaviour
    {
        public const string shieldTag = "shield";

        #region refs
        public PlayManager.PlayerPressManager playerPressManager;
        public Transform shieldTrans;
        #endregion

        [System.Serializable]
        public class ShieldSettings
        {
            public float shieldRotateSpeed = 10;
            public float shieldUpLerpSpeed = 10;
            public float shieldDownLerpSpeed = 10;
        }
        public ShieldSettings shieldSettings;


        private bool shield_was_up = false;
        private Vector2 start_pos;


        #region handy refs
        Gameplay.Player.Trinon trinon => References.PlayerInfo.parts.trinon;
        Gameplay.Player.Pivot pivot => References.PlayerInfo.parts.pivot;
        #endregion

        private void OnEnable()
        {
            shieldTrans.gameObject.SetActive( false );
            playerPressManager.onDrag += OnDrag;
            playerPressManager.onPointerUp += OnPointerUp;
        }

        private void OnDisable()
        {
            playerPressManager.onDrag -= OnDrag;
            playerPressManager.onPointerUp -= OnPointerUp;
        }

        private void OnDrag(PointerEventData pointerEventData)
        {
            if (!shield_was_up)
            {
                // shield up Start

                shield_was_up = true;
                start_pos = pointerEventData.position;

                // disable trinon fully
                trinon.enabled = false;

                shieldTrans.gameObject.SetActive( true );

                StopAllCoroutines();
                StartCoroutine( ShieldUp() );

                SetShieldRotation();
            }
            else
            {
                // shield up Update
                SetShieldRotation();
            }
        }

        private void OnPointerUp(PointerEventData pointerEventData)
        {
            if (shield_was_up)
            {
                // on shield Down
                shield_was_up = false;

                shieldTrans.gameObject.SetActive( false );
                StopAllCoroutines();
                StartCoroutine( ShieldDown() );

                // enable trinon
                trinon.enabled = true;
            }

        }

        private void SetShieldRotation()
        {
            // set pivot rotation
            Vector2 delta_pos = Inputs.InputHandler.current.getTouchPosition() - start_pos;
            //shieldTrans.up = Vector3.Lerp( shieldTrans.up, delta_pos, shieldSettings.shieldRotateSpeed * Time.deltaTime );
            pivot.transform.up = Vector3.Lerp( shieldTrans.up, delta_pos, shieldSettings.shieldRotateSpeed * Time.deltaTime );

        }

        IEnumerator ShieldUp()
        {
            shieldTrans.localScale = Vector3.zero;

            do
            {
                shieldTrans.localScale += Vector3.one * shieldSettings.shieldUpLerpSpeed * Time.deltaTime;
                yield return null;
            } while (shieldTrans.localScale.x < 1);

            // absolute
            shieldTrans.localScale = Vector3.one;
        }
        IEnumerator ShieldDown()
        {
            do
            {
                shieldTrans.localScale -= Vector3.one * shieldSettings.shieldDownLerpSpeed * Time.deltaTime;
                yield return null;
            } while (shieldTrans.localScale.x > 0);

            // absolute
            shieldTrans.localScale = Vector3.zero;
        }
    }
}