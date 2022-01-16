using ThemeSystem;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ThemeManager))]
public class ThemeManagerEditor : Editor
{
    private void OnEnable()
    {
        var tar = target as ThemeManager;
        tar.Save();
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tar = target as ThemeManager;
        if (GUILayout.Button("Save"))
        {
            tar.Save();
        }
        if (GUILayout.Button("Load"))
        {
            tar.Load();
        }
    }
}
