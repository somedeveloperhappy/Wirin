using Enemies;
using UnityEditor;
using UnityEngine;

namespace EnemyOnDamageSettings.Editor
{
	[CustomEditor(typeof(EnemyLifeShow))]
	public class EnemyLifeShowEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Resolve fields autmatically"))
			{
				var tar = target as EnemyLifeShow;

				if (tar == null) return;
				
				serializedObject.FindProperty("enemy").objectReferenceValue = tar.GetComponentInParent<Enemy>();
				serializedObject.ApplyModifiedProperties();
			}
		}
	}
}