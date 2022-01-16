using FlatTheme.UpgradeMenu;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShineUIParticleFx))]
public class ShineUIParticleFxEditor : UnityEditor.Editor
{
    ShineUIParticleFx tar;

    private void OnEnable()
    {
        tar = target as ShineUIParticleFx;
        tar.UpdateBoundry();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Awake"))
        {
            tar.Awake();
        }
    }



    private void OnSceneGUI()
    {
        Handles.color = Color.green;

        Handles.DrawAAPolyLine(
            width: 10,
            tar.transform.TransformPoint(new Vector2(tar.boundry.xMin, tar.boundry.yMin)),
            tar.transform.TransformPoint(new Vector2(tar.boundry.xMin, tar.boundry.yMax)),
            tar.transform.TransformPoint(new Vector2(tar.boundry.xMax, tar.boundry.yMax)),
            tar.transform.TransformPoint(new Vector2(tar.boundry.xMax, tar.boundry.yMin)),
            tar.transform.TransformPoint(new Vector2(tar.boundry.xMin, tar.boundry.yMin))
        );
    }
}

