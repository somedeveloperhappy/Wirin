using UnityEngine;
using UnityEditor;
using FlatTheme.MainMenuUI;

[CustomEditor (typeof (MainMenuCanvasFunctions))]
public class MainMenuCanvasFunctionsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();


		if (GUILayout.Button ("Auto Resolve"))
		{
			var tar = target as MainMenuCanvasFunctions;

			Undo.RecordObject(tar, "main menu canvas functions auto resolver");
			
			tar.canvasGroup = tar.GetComponent<CanvasGroup> ();
			tar.canvasBase = tar.GetComponent<CanvasSystem.CanvasBase> ();
			tar.graphicRaycaster = tar.GetComponent<UnityEngine.UI.GraphicRaycaster> ();
			EditorUtility.SetDirty (serializedObject.targetObject);
			serializedObject.ApplyModifiedProperties ();
			serializedObject.Update ();
		}
	}
}
