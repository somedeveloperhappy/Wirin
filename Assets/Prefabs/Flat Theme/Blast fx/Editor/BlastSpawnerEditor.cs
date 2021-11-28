using UnityEngine;
using UnityEditor;

namespace FlatVFX
{
	[CustomEditor (typeof (BlastSpawner))]
	public class BlastSpawnerEditor : Editor
	{
		public override void OnInspectorGUI() {
			
			serializedObject.Update();
			base.OnInspectorGUI ();

			if (GUILayout.Button ("Sort")) 
			{
				(target as BlastSpawner).SortBlastsBasedOnMaxT ();
				serializedObject.ApplyModifiedProperties ();
			}
		}
	}

}
