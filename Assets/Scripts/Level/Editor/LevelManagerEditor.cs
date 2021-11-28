using UnityEngine;
using UnityEditor;
using LevelManaging;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    private void OnEnable() 
    {
        (target as LevelManager).LoadLevelNumberFromPrefs();
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        EditorGUILayout.Space(10);
        if (GUILayout.Button("Clear playerPrefs", GUILayout.Height(25))) {
            PlayerPrefs.DeleteAll();
        }
    }
}
