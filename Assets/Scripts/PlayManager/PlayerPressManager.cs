using Gameplay;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PlayManager
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public class PlayerPressManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public bool canGetInputs = true;

        [SerializeField] private MultiDirectionalGesture m_addBulletCountGesture;
        [SerializeField] private MultiDirectionalGesture m_subBulletCountGesture;

        public UnityEngine.UI.Image[] bullet_indicators;

        [HideInInspector] public float stateDuration;


        #region references
        public Gameplay.Player.Shield shield;
        #endregion


        #region parameters for controllign inputs
        enum PointerState
        {
            None,       // pointer is just up
            TooSoon,    // time has not yet reached POINTER_DOWNTIME_THRESHOLD
            Drag,       // it's dragged before time reached POINTER_DOWNTIME_THRESHOLD
            Idle        // it did not drag before time reached POINTER_DOWNTIME_THRESHOLD
        }
        PointerState m_pointerState;
        private PointerEventData m_pointerEventData; // used for onUpdate methods's argument
        private bool _pointerDown = false;
        private bool _pointerDragged = false;

        /// <summary>
        /// for how much time the pointer shouldnt drag
        /// </summary>
        const float POINTER_DOWNTIME_THRESHOLD = 0.03f;
        float press_start_time = 0;
        #endregion




        #region pointer overrides
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            m_pointerEventData = eventData;
            _pointerDown = true;
        }
        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            m_pointerEventData = eventData;
            _pointerDown = false;
        }
        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            m_pointerEventData = eventData;
            _pointerDragged = true;
        }
        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            m_pointerEventData = eventData;
            _pointerDragged = false;
        }
        void IDragHandler.OnDrag(PointerEventData eventData) => m_pointerEventData = eventData;
        #endregion


        private void Update()
        {
            if (_pointerDown && canGetInputs)
            {
                if (m_pointerState == PointerState.None)
                {
                    if (_pointerDragged)
                    {
                        // start of drag
                        m_pointerState = PointerState.Drag;
                        dragStart(m_pointerEventData);
                    }
                    else
                    {
                        // starting of real pointer down. 
                        press_start_time = Time.timeSinceLevelLoad;
                        m_pointerState = PointerState.TooSoon;
                    }
                }
                else if (m_pointerState == PointerState.TooSoon)
                {
                    Debug.Log($"is dragging ? {_pointerDragged}");

                    if (_pointerDragged)
                    {
                        // start of drag
                        dragStart(m_pointerEventData);
                        m_pointerState = PointerState.Drag;
                    }
                    else
                    {
                        // check if time is passed the threshold
                        if (Time.timeSinceLevelLoad - press_start_time >= POINTER_DOWNTIME_THRESHOLD)
                        {
                            // time is passed. 
                            // start of enhanced on pointer down
                            pointerDownStart(m_pointerEventData);
                            m_pointerState = PointerState.Idle;
                        }
                        else
                        {
                            // update pointer up
                            pointerUpUpdate(m_pointerEventData);
                        }
                    }
                }
                else if (m_pointerState == PointerState.Idle)
                {
                    // enhanced update on pointer down
                    pointerDownUpdate(m_pointerEventData);
                }
                else if (m_pointerState == PointerState.Drag)
                {
                    // update of drag
                    dragUpdate(m_pointerEventData);
                }
            }
            else // _pointerDown == false
            {
                if (m_pointerState == PointerState.None)
                {
                    // update of poiner up
                    pointerUpUpdate(m_pointerEventData);
                }
                else if (m_pointerState == PointerState.Idle)
                {
                    // end of enhanced pointer down
                    pointerUpStart(m_pointerEventData);
                    m_pointerState = PointerState.None;
                }
                else if (m_pointerState == PointerState.Drag)
                {
                    // end of drag
                    dragEnd(m_pointerEventData);
                    m_pointerState = PointerState.None;
                }
                else if (m_pointerState == PointerState.TooSoon)
                {
                    // ended before even started enhanced pointer down
                    m_pointerState = PointerState.None;
                }
            }
        }

        private static Exception UnusualInputException()
        {
            Debug.Break();
            return new Exception("Something unusual happened with inputs! stopping the game now");
        }

        #region on enhance (actually ingame) pointers
        private void pointerDownStart(PointerEventData eventData)
        {
            OnPlayerPress.ForeachInstance(pp => pp.OnPressDown(stateDuration));
            stateDuration = 0;
        }
        private void pointerDownUpdate(PointerEventData eventData)
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressDownUpdate());
        }
        private void pointerUpStart(PointerEventData eventData)
        {
            OnPlayerPress.ForeachInstance(pp => pp.OnPressUp(stateDuration));
            stateDuration = 0;
        }
        private void pointerUpUpdate(PointerEventData eventData)
        {
            stateDuration += Time.deltaTime;
            OnPlayerPress.ForeachInstance(pp => pp.OnPressUpUpdate());
        }
        private void dragStart(PointerEventData eventData)
        {
            shield.ShieldUp(eventData);

            // bullet gesture reset
            m_addBulletCountGesture.Reset();
            m_subBulletCountGesture.Reset();
        }
        private void dragUpdate(PointerEventData eventData)
        {
            shield.ShildUpdate(eventData);


            // bullet adding gesture
            m_addBulletCountGesture.AddDeltaInfo(eventData.delta);
            if (m_addBulletCountGesture.turnIsComplete)
            {
                if (References.playerInfo.CanAddTrinon())
                    References.playerInfo.InstantiacteNewTrinon();
                m_addBulletCountGesture.Reset();
            }

            // bullet subtracting gesture
            m_subBulletCountGesture.AddDeltaInfo(eventData.delta);
            if (m_subBulletCountGesture.turnIsComplete)
            {
                Debug.Log("bullet add gesture completed");
                if (References.playerInfo.CanRemoveTrinon())
                    References.playerInfo.RemoveLastTrinon();
                m_subBulletCountGesture.Reset();
            }

            // bullet count images update color
            for (int i = 0; i < References.playerInfo.GetShootings().trinonMaxCount; i++)
            {
                bullet_indicators[i].color = i < References.playerInfo.parts.trinons.Count ? Color.green : Color.white;
            }
        }
        private void dragEnd(PointerEventData eventData)
        {
            Debug.Log("drag end");
            shield.ShieldDown();

            // bullet gesture reset
            m_addBulletCountGesture.Reset();
            m_subBulletCountGesture.Reset();
        }
        #endregion
    }
}