using UnityEngine;


namespace PlayManagement
{
    public partial class PlayerPressManager : MonoBehaviour
    {
        
        bool wasPressed = false; // for checking if in previous frame the pivot was pressed
        
        float pressedDuration = 0;
        
        public float pressableRange = 4;
        
        private void Update()
        {
            InputUpdates();
        }
        
        private void InputUpdates()
        {
            if(InputGetter.isPoinerDown)
            {
                var pointerpos = InputGetter.GetPointerWorldPosition();
                
                // check for distance of the pointer and the center of screen
                if(Vector2.Distance(pointerpos, Vector2.zero) <= pressableRange) {
                    
                    // check if its the first time
                    if(!wasPressed) {
                        wasPressed = true;
                        IOnPlayerPressHelper.ForeachInstance((pp) => pp.OnPressDown(pressedDuration) );
                        pressedDuration = 0;
                        
                    } else {
                        
                        // update 
                        pressedDuration += Time.deltaTime;
                        IOnPlayerPressHelper.ForeachInstance((pp) => pp.OnPressDownUpdate() );
                        
                    }
                    
                    
                }
                
            } else {
                
                // check if it was pressed before
                if(wasPressed) {
                    wasPressed = false;
                    IOnPlayerPressHelper.ForeachInstance((pp) => pp.OnPressUp(pressedDuration) );
                    pressedDuration = 0;
                    
                } else {
                    
                    // update
                    pressedDuration += Time.deltaTime;
                    IOnPlayerPressHelper.ForeachInstance((pp) => pp.OnPressUpUpdate() );
                    
                }
                
            }
            
        }
    }
}