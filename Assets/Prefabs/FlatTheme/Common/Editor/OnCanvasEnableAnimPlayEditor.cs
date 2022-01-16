
using FlatTheme.Common;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OnCanvasEnabledAnimPlay))]
public class OnCanvasEnableAnimPlayEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Auto Resolve"))
        {
            var tar = target as OnCanvasEnabledAnimPlay;
            tar.AutoResolve();
        }
    }
}

