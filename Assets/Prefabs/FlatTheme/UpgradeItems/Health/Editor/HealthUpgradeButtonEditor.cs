using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UpgradeSystem.UpgradeItems.Health))]
public class HealthUpgradeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // draw equation 
        GUILayout.Box(FunctionGrapher.GetEquationTexture(EvaluateHealth, Vector2.zero, new Vector2(5, 3), 200, 200),
            GUILayout.Width(200),
            GUILayout.Height(200));
    }

    static public float EvaluateHealth(float value)
    {
        float f = (float)System.Math.Exp(value / 14f);
        float g = (100f * value) / 60f;
        return Mathf.Lerp(f, g, value / 60f) + 1;
    }
}
