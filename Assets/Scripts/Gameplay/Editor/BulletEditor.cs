using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : Editor
{
    private void OnSceneGUI()
    {
        var tar = target as Bullet;
        
        Handles.color = Color.green;
        Handles.DrawWireCube(tar.transform.position, Vector3.one * (float)(tar.GetType().GetField("boundryRange").GetValue(tar)));
    }
}