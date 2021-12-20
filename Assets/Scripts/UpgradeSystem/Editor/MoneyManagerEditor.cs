using UnityEditor;
using UnityEngine;
using UpgradeSystem;

[CustomEditor(typeof(MoneyManager))]
public class MoneyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tar = target as MoneyManager;

        if (GUILayout.Button("Load"))
        {
            tar.Load();
        }

        if (GUILayout.Button("Save"))
        {
            tar.Save();
        }
    }
}
