using UnityEngine;
using UnityEditor;
using PlayManagement;

[CustomEditor(typeof(PlayerPressManager))]
public class PlayerPressManagerEditor : Editor
{
    private void OnSceneGUI() {
        var tar = target as PlayerPressManager;
        Handles.color = new Color(1, 0, 0, 0.2f);
        Handles.DrawSolidDisc(Vector3.zero, Vector3.forward, tar.pressableRange);
    }
}