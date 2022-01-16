using Gameplay.Player;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInfo))]
public class PlayerInfoEditor : Editor
{
    PlayerInfo tar;
    void OnEnable()
    {
        tar = target as PlayerInfo;
    }
    public override void OnInspectorGUI()
    {


        base.OnInspectorGUI();
        if (GUILayout.Button("Resolve Fields Automatically"))
        {

            var parts_prop = serializedObject.FindProperty(nameof(tar.parts));
            parts_prop.FindPropertyRelative(nameof(tar.parts.pivot)).objectReferenceValue =
                tar.GetComponentInChildren<Pivot>();
            parts_prop.FindPropertyRelative(nameof(tar.parts.mainTrinon)).objectReferenceValue =
                tar.GetComponentInChildren<Trinon>();
            serializedObject.ApplyModifiedProperties();

            // draw direction line

        }

    }
    public void OnSceneGUI()
    {
        Handles.color = Color.red;
        Handles.DrawAAPolyLine(
            width: 20,
            tar.transform.position,
            tar.transform.position + (Vector3)tar.Direction * 2);
    }
}
