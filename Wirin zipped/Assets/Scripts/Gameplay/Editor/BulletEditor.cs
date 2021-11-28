using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : Editor
{
    private void OnSceneGUI() {
        var tar = target as Bullet;

        float boundryRange = (float) (tar.GetType().GetField("boundryRange",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(tar));

        Handles.color = Color.green;
        Handles.DrawWireCube(tar.transform.position, Vector3.one * boundryRange);
    }
}