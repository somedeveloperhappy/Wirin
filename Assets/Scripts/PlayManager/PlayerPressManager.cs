using Gameplay;
using Statics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayManager
{
    [RequireComponent( typeof( UnityEngine.UI.Image ) )]
    public class PlayerPressManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler
    {
        public bool canGetInputs = true;

        [Tooltip( "the duration of the current state ( press, release)" )]
        [HideInInspector] public float stateDuration;

        [Tooltip( "for how much time the pointer shouldnt move much" )]
        public float pointerDownTimeThreshold = 0.15f;


        private bool wasPressed; // for checking if in previous frame the pivot was pressed
        bool is_pointer_in = false;
        float pointer_start_t = 0; // the time when the pointer started 


        #region events
        public System.Action<PointerEventData> onPointerDown;
        public System.Action<PointerEventData> onPointerUp;
        public System.Action<PointerEventData> onPointerExit;
        public System.Action<PointerEventData> onDrag;
        #endregion


        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            is_pointer_in = true; pointer_start_t = Time.realtimeSinceStartup;
            onPointerDown?.Invoke( eventData );
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            is_pointer_in = false;
            onPointerUp?.Invoke( eventData );
        }
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            is_pointer_in = false;
            onPointerExit?.Invoke( eventData );
        }
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log( "On Begin Drag" );
            is_pointer_in = false;
        }
        void IDragHandler.OnDrag(PointerEventData eventData) => onDrag?.Invoke( eventData );

        private void Update()
        {
            InputUpdates();
        }
        private void InputUpdates()
        {
            if (is_pointer_in && canGetInputs && Time.realtimeSinceStartup - pointer_start_t >= pointerDownTimeThreshold)
            {
                if (!wasPressed)
                {
                    wasPressed = true;
                    // on down start
                    downStart();
                    downUpdate();
                }
                else
                {
                    // on down update 
                    downUpdate();
                }
            }
            else
            {
                if (wasPressed)
                {
                    wasPressed = false;
                    // on up start
                    upStart();
                    upUpdate();
                }
                else
                {
                    // on up update
                    upUpdate();
                }
            }
        }


        float start_pos, start_t;
        private void downStart()
        {
            OnPlayerPress.ForeachInstance( pp => pp.OnPressDown( stateDuration ) );
            stateDuration = 0;
        }

        private void downUpdate()
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance( pp => pp.OnPressDownUpdate() );
        }

        private void upStart()
        {
            OnPlayerPress.ForeachInstance( pp => pp.OnPressUp( stateDuration ) );
            stateDuration = 0;
        }


        private void upUpdate()
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance( pp => pp.OnPressUpUpdate() );
        }

    }
}