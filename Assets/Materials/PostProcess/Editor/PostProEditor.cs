using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PostPro))]
public class PostProEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();

		var tar = target as PostPro;

		if (tar.pp_mat)
		{
			// var mat_prop = serializedObject.FindProperty (nameof (tar.pp_mat));
			var mat = new SerializedObject(tar.pp_mat);
			EditorGUILayout.PropertyField(mat.GetIterator());
		}
	}
}
