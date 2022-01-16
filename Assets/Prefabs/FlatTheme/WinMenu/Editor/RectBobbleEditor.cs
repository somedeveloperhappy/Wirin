using FlatTheme.WinMenu;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RectBobble))]
public class RectBobbleEditor : Editor
{
    private void OnSceneGUI()
    {
        var tar = target as RectBobble;
        var col = Color.green;
        col.a = 0.2f;
        Handles.color = col;

        Handles.DrawSolidDisc(tar.transform.position, Vector3.back, tar.settings.movingDistance);
    }
}
