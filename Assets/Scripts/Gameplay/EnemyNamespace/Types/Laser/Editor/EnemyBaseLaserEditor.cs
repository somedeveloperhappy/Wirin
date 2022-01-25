using Gameplay.EnemyNamespace.Types.Laser;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBaseLaser))]
public class EnemyBaseLaserEditor : Editor
{
    EnemyBaseLaser tar;
    private void OnEnable() {
        tar = target as EnemyBaseLaser;
    }
    private void OnSceneGUI() 
    {
        var col = Color.red;
        col.a = 0.2f;
        Handles.color = col;
        Handles.DrawSolidDisc(tar.transform.position, Vector3.forward, tar.settings.laserDistance);
    }
}