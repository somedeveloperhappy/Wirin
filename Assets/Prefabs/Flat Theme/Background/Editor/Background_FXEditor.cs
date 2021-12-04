using UnityEditor;
using UnityEngine;

namespace FlatVFX
{
	[CustomEditor(typeof(Background_FX))]
	public class Background_FXEditor : Editor
	{
		private void OnSceneGUI()
		{
			var tar = target as Background_FX;

			Handles.color = Color.green;
			foreach (var model in tar.models)
			{
				if (model.meshRenderer == null) return;

				Handles.DrawWireDisc(model.rotatingPivot, Vector3.back,
					Vector2.Distance(model.meshRenderer.transform.position, model.rotatingPivot));

				Handles.DrawSolidDisc(model.rotatingPivot, Vector3.back, 0.5f);


				EditorGUI.BeginChangeCheck();
				var p = Handles.PositionHandle(model.rotatingPivot, Quaternion.identity);
				if (EditorGUI.EndChangeCheck())
				{
					model.rotatingPivot = p;
					serializedObject.ApplyModifiedProperties();
				}

			}
		}
	}
}