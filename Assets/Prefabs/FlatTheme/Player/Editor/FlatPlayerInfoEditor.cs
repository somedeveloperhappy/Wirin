
using FlatTheme.Player;
using UnityEditor;

[CustomEditor(typeof(FlatPlayerInfo))]
public class FlatPlayerInfoEditor : PlayerInfoEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
    private void OnSceneGUI()
    {
        base.OnSceneGUI();
    }
}

