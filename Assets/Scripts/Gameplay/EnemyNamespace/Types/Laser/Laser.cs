using UnityEngine;

namespace Gameplay.EnemyNamespace.Types.Laser
{
    public class Laser : MonoBehaviour
    {
        public void StartShooting() => gameObject.SetActive(true);
        public void StopShooting() => gameObject.SetActive(false);
    }
}