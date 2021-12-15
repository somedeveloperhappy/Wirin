using UnityEditor;
using UnityEngine;

namespace Gameplay.Player.Editor
{
    [CustomEditor(typeof(PlayerInfo))]
    public class PlayerInfoEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Resolve Fields Automatically"))
            {
                var tar = target as PlayerInfo;

                var parts_prop = serializedObject.FindProperty(nameof(tar.parts));
                parts_prop.FindPropertyRelative(nameof(tar.parts.pivot)).objectReferenceValue =
                    tar.GetComponentInChildren<Pivot>();
                parts_prop.FindPropertyRelative(nameof(tar.parts.trinon)).objectReferenceValue =
                    tar.GetComponentInChildren<Trinon>();
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}