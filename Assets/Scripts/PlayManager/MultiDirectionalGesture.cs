using UnityEngine;

namespace PlayManager
{
    [System.Serializable]
    public class MultiDirectionalGesture
    {
        [SerializeField] Vector2[] directions;
        public bool turnIsComplete { get; private set; } = false;
        public int lastIndex { get; private set; } = 0;
        public int done_directions = 0;

        public void Reset()
        {
            lastIndex = 0;
            done_directions = 0;
            turnIsComplete = false;
        }
        public void AddDeltaInfo(Vector2 delta)
        {
            if (turnIsComplete) return;

            var ind = getNearestIndex(delta);

            if (ind == lastIndex + 1 || (done_directions == 0 && ind == 0))
            {
                // it's gone accordingly, assign it
                lastIndex = ind;
                //Debug.Log( $"Drag done for [{done_directions}]: {directions[lastIndex]}" );

                // check for full gesture
                done_directions++;
                if (done_directions >= directions.Length)
                {
                    // full 
                    turnIsComplete = true;
                }
            }
            else if (ind != lastIndex)
            {
                // it's not right. cancel it
                Reset();
            }
        }

        private int getNearestIndex(Vector2 delta)
        {
            var ind = -1;
            float min_dist = float.MaxValue;
            for (int i = 0; i < directions.Length; i++)
            {
                int current_ind = (i + lastIndex) % directions.Length;
                float dist = Vector2.Distance(delta, directions[current_ind]);
                if (dist < min_dist)
                {
                    min_dist = dist;
                    ind = current_ind;
                }
            }
            return ind;
        }
    }
}