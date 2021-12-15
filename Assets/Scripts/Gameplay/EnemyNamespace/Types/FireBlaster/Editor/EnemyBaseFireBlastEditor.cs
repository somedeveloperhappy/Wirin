using Gameplay.EnemyNamespace.Types.FireBlaster;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBaseFireBlast))]
public class EnemyBaseFireBlastEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Resolve Fields Automatically"))
        {
            var tar = target as EnemyBaseFireBlast;
            var settings_prop = serializedObject.FindProperty(nameof(tar.settings));
            var cannonSettings_prop = settings_prop.FindPropertyRelative(nameof(tar.settings.cannonSettings));

            var cannons = tar.GetComponentsInChildren<Cannon>();

            if (cannonSettings_prop.arraySize < cannons.Length)
                cannonSettings_prop.arraySize = cannons.Length;

            for (int i = 0; i < cannons.Length; i++)
            {
                cannonSettings_prop.GetArrayElementAtIndex(i).FindPropertyRelative("cannon")
                    .objectReferenceValue = cannons[i];
            }
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void OnSceneGUI()
    {
        var tar = target as EnemyBaseFireBlast;
        Handles.color = Color.green;

        Handles.DrawWireDisc(tar.transform.position, Vector3.back, tar.settings.distanceToShoot, 3);


    }
}
