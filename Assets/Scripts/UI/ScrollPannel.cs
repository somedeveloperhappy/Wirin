using System.Collections.Generic;
using UnityEngine;
using CanvasSystem;
using UnityEngine.EventSystems;

namespace UI
{
    public partial class ScrollPannel : MonoBehaviour, IOnCanvasEnabled, IOnCanvasDisabled, IDragHandler
    {
        [Tooltip( "The target moving Object (The parent of the scrollable items. it SHOULD NOT be this transform)" )]
        public RectTransform targetTransform;
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
        float velocity = 0;
        #endregion


        private void OnEnable()
        {
            AutoSetXBoundries();
        }

        private void Update()
        {
            //Debug.Log( "input delta : " + velocity );
            MoveAllElements();
            velocity = Mathf.MoveTowards( velocity, 0, scrolMovement.cooldownSpeed * Time.unscaledDeltaTime );
        }
        public void OnDrag(PointerEventData eventData)
        {
            velocity = eventData.delta.x;
            Debug.Log( "On Pointer Down" );
        }

        private void MoveAllElements()
        {
            if (velocity == 0) return;

            // x boundry checking 
            if (velocity > 0 && velocity + targetTransform.position.x + scrolXBoundries.min > 0)
                velocity = -targetTransform.position.x - scrolXBoundries.min;
            if (velocity < 0 && velocity + targetTransform.position.x + scrolXBoundries.max < Screen.width)
                velocity = -targetTransform.position.x - scrolXBoundries.max + Screen.width;


            var moving = Vector3.right * velocity * scrolMovement.speed * Time.unscaledDeltaTime;
            targetTransform.position += moving;
            DisableOutsideView();
        }
        float GetInputMoveDelta()
        {
            if (Inputs.InputHandler.current.isTouchDown())
                return Inputs.InputHandler.current.getDelta().x;
            return 0;
        }

        public void OnCanvasEnable() => this.enabled = true;
        public void OnCanvasDisable() => this.enabled = false;
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

            foreach (Transform trans in targetTransform)
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
                    targetTransform.position + (Vector3)transformSettings.ofsset
                    + Vector3.right * (last_dist + transformSettings.distanceX) * i;
                last_dist = scrollables[i].viewDistance * 2;
            }
        }

        public void DisableOutsideView()
        {
            //Debug.Log( $"auto disabling scrollable buttons. screen height : {Screen.height} height : {Screen.width}" );

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

            if (!targetTransform)
                targetTransform = GetComponent<RectTransform>();
            scrolXBoundries.min = Mathf.Min( -targetTransform.position.x, targetTransform.position.x + minPos - distance_min * 1.5f );
            scrolXBoundries.max = Mathf.Max( Screen.width - targetTransform.position.x, targetTransform.position.x + maxPos + distance_max * 1.5f );
        }


    }
}
