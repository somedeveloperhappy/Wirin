using Gameplay.EnemyNamespace.Types.FireBlaster;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cannon))]
public class CannonEditor : Editor
{
    bool editingShootPos = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (editingShootPos)
        {
            if (GUILayout.Button("Exit Editing Shoot Position"))
            {
                editingShootPos = false;
            }
        }
        else
        {
            if (GUILayout.Button("Edit Shoot Position"))
            {
                editingShootPos = true;
            }
        }

        var tar = target as Cannon;

        if (GUILayout.Button("Resolve Automatically"))
        {
            var result = tar.GetComponentInParent<EnemyBaseFireBlast>();
            serializedObject.FindProperty("enemy").objectReferenceValue = result;
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }

    private void OnSceneGUI()
    {
        var tar = target as Cannon;

        Handles.color = Color.red;
        Handles.DrawSolidDisc(tar.GetShootPosition(), Vector3.back, 0.03f);
        Handles.DrawLine(tar.GetShootPosition(), (Vector3)tar.GetShootPosition() + tar.transform.up * 0.2f, 3);

        if (!editingShootPos) return;

        EditorGUI.BeginChangeCheck();
        serializedObject.FindProperty("shootPosOffset").vector2Value =
            Handles.PositionHandle(tar.GetShootPosition(), Quaternion.identity) - tar.transform.position;
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

    }
}
