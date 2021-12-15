using Gameplay.Player;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Editor
{
    [CustomEditor(typeof(Trinon))]
    public class TrinonEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            var tar = target as Trinon;
            Handles.color = Color.red;
            Handles.DrawSolidDisc(tar.GetBulletPositionInWorld(), Vector3.forward, 0.03f);
        }
    }
}