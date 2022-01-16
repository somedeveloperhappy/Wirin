
using FlatTheme.UpgradeMenu;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DirectionalUIParticleFx))]
public class DirectionalUIParticleFxEditor : UnityEditor.Editor
{
    DirectionalUIParticleFx tar;
    private void OnEnable()
    {
        tar = target as DirectionalUIParticleFx;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.blue;

        createLine(tar.startRotationRange.min);
        createLine(tar.startRotationRange.max);

        void createLine(float val)
        {
            Handles.DrawAAPolyLine(
                width: 10,
                tar.transform.position,
                tar.transform.position +
                    (tar.transform.up * Mathf.Cos(val) + tar.transform.right * Mathf.Sin(-val)
                    ) * 100);
        }
    }
}

