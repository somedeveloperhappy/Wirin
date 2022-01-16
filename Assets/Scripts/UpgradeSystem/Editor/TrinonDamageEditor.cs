using UnityEditor;
using UpgradeSystem;
using UpgradeSystem.UpgradeItems;

[CustomEditor(typeof(TrinonLowDamage))]
public class TrinonDamageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var tar = target as MoneyManager;

    }
}
