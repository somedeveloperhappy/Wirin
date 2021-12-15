using UnityEngine;

namespace Management.Helper
{
    [CreateAssetMenu]
    public class LineSegments : ScriptableObject
    {

        public float maximumX;
        [SerializeField] private Vector2[] points;

        public Vector2[] GetPoints()
        {
            return points;
        }

        /// <returns>the sum of distances of all points</returns>
        public void CalculateMaxX()
        {
            maximumX = 0;
            for (ushort i = 0; i < points.Length - 1; i++) maximumX += Vector2.Distance(points[i], points[i + 1]);

            maximumX += Vector2.Distance(points[points.Length - 1], points[0]);
        }

        public Vector2 GetPoint(float X)
        {
            var indx = 0;

            Vector2 nextPoint()
            {
                return points[(indx + 1) % points.Length];
            }

            Vector2 currentPoint()
            {
                return points[indx];
            }

            while (true)
            {
                var dst = Vector2.Distance(nextPoint(), currentPoint());

                if (dst >= X) return currentPoint() + (nextPoint() - currentPoint()).normalized * X;

                X -= dst;

                indx++;

                if (indx >= points.Length)
                {
                    Debug.LogError("X was too much. returning point 1");
                    return points[0];
                }
            }
        }
    }
}