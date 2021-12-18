using System.Collections.Generic;
using UnityEngine;


namespace UI
{
    public partial class ScrollPannel : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        public SimpleScripts.MinMax touchYlimit;
        public SimpleScripts.MinMax scrolXBoundries;

        [System.Serializable]
        public struct TransformSettings
        {
            [HideInInspector]
            public Vector2 ofsset;
            public float distanceX;
        }
        public TransformSettings transformSettings;

        [System.Serializable]
        public struct ScrolMovement
        {
            public float speed;
            public float cooldownSpeed;
        }
        public ScrolMovement scrolMovement;

        public Scrollable[] scrollables;


        #region private local vars
        float mouse_last_pos_x = float.MinValue;
        float vel = 0;
        RectTransform m_rectTransform;
        #endregion


        private void Awake()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }
        private void OnEnable()
        {
            m_rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            var input_delta = GetInputMoveDelta();


            if (input_delta != 0)
            {
                Debug.Log( "input delta : " + input_delta );
                vel = input_delta;
            }
            else
                vel = Mathf.MoveTowards( vel, 0, scrolMovement.cooldownSpeed * Time.unscaledDeltaTime );

            MoveAllElements();
        }

        private void MoveAllElements()
        {
            if (vel == 0) return;

            // x boundry checking 
            if (vel > 0 && scrolXBoundries.min + transform.position.x > 0)
                return;
            if (vel < 0 && scrolXBoundries.max + transform.position.x < Screen.width)
                return;


            // var moving = Vector3.right * vel * scrolMovement.speed * Time.deltaTime;
            var moving = Vector3.right * vel * scrolMovement.speed * Time.unscaledDeltaTime;

            m_rectTransform.position += moving;

            DisableOutsideView();
        }

        float GetInputMoveDelta()
        {
            if (Inputs.InputHandler.current.isTouchDown())
                return Inputs.InputHandler.current.getDelta().x;
            return 0;
        }

        bool isYInsideTouchArea(float y) =>
            y >= touchYlimit.min + m_rectTransform.position.y &&
            y <= touchYlimit.max + m_rectTransform.position.y;

        public void OnCanvasEnable()
        {
            this.enabled = true;
            ReAutoArrangeAll();
        }
        public void OnCanvasDisable()
        {
            this.enabled = false;
        }
        public void ReAutoArrangeAll()
        {
            GetAutoReferences();
            RepositionSubs();
            DisableOutsideView();
            AutoSetXBoundries();
        }


        public void GetAutoReferences()
        {
            List<Scrollable> r = new List<Scrollable>();

            foreach (Transform trans in transform)
            {
                if (trans.TryGetComponent<Scrollable>( out Scrollable scrollable ))
                {
                    r.Add( scrollable );
                }
            }
            scrollables = r.ToArray();
        }

        public void RepositionSubs()
        {
            float last_dist = 0;
            for (int i = 0; i < scrollables.Length; i++)
            {
                scrollables[i].transform.position =
                    transform.position + (Vector3)transformSettings.ofsset
                    + Vector3.right * (last_dist + transformSettings.distanceX) * i;
                last_dist = scrollables[i].viewDistance * 2;
            }
        }

        public void DisableOutsideView()
        {
            Debug.Log( $"auto disabling scrollable buttons. screen height : {Screen.height} height : {Screen.width}" );

            foreach (var sc in scrollables)
            {
                if (sc.isOutsideView())
                {
                    if (sc.gameObject.activeSelf)
                        sc.gameObject.SetActive( false );
                }
                else if (!sc.gameObject.activeSelf)
                {
                    sc.gameObject.SetActive( true );
                }
            }
        }

        public void AutoSetXBoundries()
        {
            float minPos = float.MaxValue, maxPos = float.MinValue;
            float distance_min = 0, distance_max = 0;

            foreach (var s in scrollables)
            {
                float x = s.transform.position.x;
                if (x < minPos) { minPos = x; distance_min = s.viewDistance; }
                if (x > maxPos) { maxPos = x; distance_max = s.viewDistance; }
            }

            if (!m_rectTransform)
                m_rectTransform = GetComponent<RectTransform>();
            scrolXBoundries.min = minPos - m_rectTransform.position.x - distance_min * 1.5f;
            scrolXBoundries.max = maxPos - m_rectTransform.position.x + distance_max * 1.5f;
        }
    }
}
