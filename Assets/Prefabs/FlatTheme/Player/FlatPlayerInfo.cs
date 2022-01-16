using Gameplay.Player;
using System;
using System.Collections;
using UnityEngine;

namespace FlatTheme.Player
{
    public class FlatPlayerInfo : Gameplay.Player.PlayerInfo
    {
        [System.Serializable]
        public class LoseSettings
        {
            public float timeDownSpeed = 1;
        }
        public LoseSettings loseSettings;

        [System.Serializable]
        public struct TrinonAddingSettings
        {
            public float speed;
        }
        public TrinonAddingSettings m_trinonAddingSettings;
        [System.Serializable]
        public struct TrinonRemoveSettings
        {
            public float scaleDownSpeed;
        }
        public TrinonRemoveSettings m_trinonRemoveSettings;


        protected override IEnumerator OnLose()
        {
            do
            {
                Time.timeScale = Mathf.Max(0, Time.timeScale - loseSettings.timeDownSpeed * Time.unscaledDeltaTime);
                yield return null;

            } while (Time.timeScale > 0);

            // absolutes
            Time.timeScale = 0;
        }

        protected override void OnAddingNewTrinon(Trinon trinon, int index, Action onMethodEnd)
        {
            StartCoroutine(OnAddingNewTrinonAsync(trinon, index, onMethodEnd));
        }
        public IEnumerator OnAddingNewTrinonAsync(Trinon trinon, int index, Action onMethodEnd)
        {
            var rot = trinon.transform.localEulerAngles;
            rot.z = 0;
            ApplyRot();

            float t = 0;
            float target = 1;
            do
            {
                target = GetLocalDirectionForTrinonOfIndex(index);
                t = Mathf.InverseLerp(0, target, rot.z);
                rot.z = Mathf.MoveTowards(rot.z, target, m_trinonAddingSettings.speed * Time.deltaTime);
                //Debug.Log($"going towards {target}. so far {rot.z}");
                ApplyRot();
                yield return null;

            } while (rot.z != target);

            onMethodEnd?.Invoke();

            void ApplyRot() => trinon.transform.localEulerAngles = rot;
        }

        protected override void OnRemoveLastTrinon(Trinon trinon, Action onMethodEnd)
        {
            StartCoroutine(OnRemoveLastTrinonAsync(trinon, onMethodEnd));
        }

        private IEnumerator OnRemoveLastTrinonAsync(Trinon trinon, Action onMethodEnd)
        {

            Vector3 speed = Vector3.one * m_trinonRemoveSettings.scaleDownSpeed;
            do
            {
                trinon.transform.localScale -= speed * Time.deltaTime;
                yield return null;
            } while (trinon.transform.localScale.sqrMagnitude > 0.2f);

            onMethodEnd?.Invoke();
        }
    }
}