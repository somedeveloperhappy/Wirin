using UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Scrollable)), CanEditMultipleObjects]
public class ScrollableEditor : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.green;
        var tar = target as Scrollable;

        var left = tar.transform.position + Vector3.left * tar.viewDistance;
        Handles.DrawAAPolyLine(
            width: 5,
            left + Vector3.down * 200, left + Vector3.up * 200);
        var right = tar.transform.position + Vector3.right * tar.viewDistance;
        Handles.DrawAAPolyLine(
            width: 5,
            right + Vector3.down * 200, right + Vector3.up * 200);
    }
}
