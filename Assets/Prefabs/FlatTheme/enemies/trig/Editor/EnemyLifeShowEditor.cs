using FlatTheme;
using Gameplay.EnemyNamespace.Types;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( EnemyLifeShow ) )]
public class EnemyLifeShowEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button( "Resolve fields autmatically" ))
        {
            var tar = target as EnemyLifeShow;

            if (tar == null) return;

            serializedObject.FindProperty( "enemyBase" ).objectReferenceValue = tar.GetComponentInParent<EnemyBase>();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
