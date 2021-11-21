using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Trinon))]
public class TrinonEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI() {
        var tar = target as Trinon;
        Handles.color = Color.red;
        Handles.DrawSolidDisc(tar.GetBulletPositionInWorld(), Vector3.forward, 0.03f);
    }
}