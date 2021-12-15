using Gameplay.EnemyNamespace.Types.FireBlaster;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FireBlastBullet))]
public class BulletEditor : Editor
{
    private void OnSceneGUI()
    {
        var tar = target as FireBlastBullet;

        var boundryRange = serializedObject.FindProperty("boundryRange").floatValue;

        Handles.color = Color.green;
        Handles.DrawWireCube(tar.transform.position, Vector3.one * boundryRange);
    }
}
