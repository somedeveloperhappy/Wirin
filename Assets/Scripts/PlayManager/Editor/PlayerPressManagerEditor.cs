using Gameplay.Player;
using UnityEditor;
using UnityEngine;

namespace PlayManager
{
    [CustomEditor(typeof(PlayerPressManager))]
    public class PlayerPressManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var tar = target as PlayerPressManager;

            if (GUILayout.Button("Auto Resolve"))
            {
                Undo.RegisterCompleteObjectUndo(serializedObject.targetObject, "Player Press Manager Auto Resolve");
                tar.shield = FindObjectOfType<Shield>();
                //EditorUtility.SetDirty( serializedObject.targetObject );
            }
        }
    }
}