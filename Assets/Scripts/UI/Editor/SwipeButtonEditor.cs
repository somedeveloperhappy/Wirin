using UI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwipeButton))]
public class SwipeButtonEditor : Editor
{
    private void OnSceneGUI()
    {
        Handles.color = Color.green;

        var tar = target as SwipeButton;
        var normalizedDirection = new Vector2(Mathf.Cos(tar.directionInDegree * Mathf.Deg2Rad), Mathf.Sin(tar.directionInDegree * Mathf.Deg2Rad));
        Handles.DrawLine(tar.transform.position, tar.transform.position + (Vector3)normalizedDirection * tar.swipeThreshold, 10);

    }
}
