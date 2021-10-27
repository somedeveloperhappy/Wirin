using UnityEngine;
using System;

public class InputGetter : MonoBehaviour
{
    
    /// <summary>
    /// returns true if the pointer is held down ( also true for the first and last frame )
    /// </summary>
    /// <value></value>
    static public bool isPoinerDown {
        get {
            // mouse check
            if(Input.GetMouseButton(0)) return true;
            // input check
            return Input.touchCount > 0;
        }
    }
    
    /// <summary>
    /// returns true only during the first frame the pointer is down
    /// </summary>
    [HideInInspector]
    static public bool isPointerJustDown = false;
    
    /// <summary>
    /// returns true only during the frame the pointer is let go
    /// </summary>
    [HideInInspector]
    static public bool isPointerJustUp = false;
    
    [HideInInspector]
    static public Vector2 pointerPosition { get; private set; }
    
    #region events
         
    /// <summary>
    /// gets called during the frames where the pointer is down
    /// </summary>
    static public event Action<Vector2> onPointerDown;
    /// <summary>
    /// gets called during the frame where the pointer is let go
    /// </summary>
    static public event Action<Vector2> onPointerUp;
    
    /// <summary>
    /// gets called only during the first frame the pointer is down
    /// </summary>
    static public event Action<Vector2> onPointerJustDown;
    
    
    #endregion
    
    #region helper vars
    bool first_frame_down = true;
    bool first_frame_up = true;
    #endregion
    
    private void Update() 
    {
        if(isPoinerDown) {
            pointer_down_update();
        } else {
            pointer_up_update();
        }
    }

    private void pointer_down_update()
    {
        // save pointer position
        pointerPosition = GetPointerPosition(0);

        if (!first_frame_up) first_frame_up = true;
        if (isPointerJustUp) isPointerJustUp = false;

        if (first_frame_down)
        {
            // on pointer down start
            isPointerJustDown = true;
            onPointerJustDown?.Invoke(pointerPosition);
        }
        else
            isPointerJustDown = false;

        first_frame_down = false;

        // happens all the frames when pointer is down
        onPointerDown?.Invoke(pointerPosition);
    }

    private void pointer_up_update()
    {
        if (!first_frame_down) first_frame_down = true;
        if (isPointerJustDown) isPointerJustDown = false;

        if (first_frame_up)
        {
            // on pointer up start
            isPointerJustUp = true;
            onPointerUp?.Invoke(pointerPosition);
        }
        else
            isPointerJustUp = false;

        first_frame_up = false;
    }



    /// <summary>
    /// returns pointer position. if none was held down, returns positive infinity
    /// </summary>
    static public Vector2 GetPointerPosition(int index = 0)
    {
        if(Input.GetMouseButton(0)) {
            //mouse position check
            return Input.mousePosition;
        }
        
        // touch check
        if(Input.touchCount > index) {
            return Input.touches[index].position;
        }
        
        return Vector2.positiveInfinity;
    }

    static public Vector2 GetPointerWorldPosition(int index = 0)
    {
        return Camera.main.ScreenToWorldPoint(pointerPosition);
    }
}