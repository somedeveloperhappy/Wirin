#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace PlayManagement
{
    public partial class PlayerPressManager
    {
        private void OnDrawGizmosSelected() {
            Handles.color = new Color(1, 0, 0, 0.2f);
            Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, pressableRange);
        }
    }
}
#endif