using UnityEditor;
using UnityEngine;

public class FontGen : EditorWindow
{


	public Font font;

	[MenuItem("Wirin/FontGen")]
	private static void ShowWindow()
	{
		var window = GetWindow<FontGen>();
		window.titleContent = new GUIContent("FontGen");
		window.Show();
	}

	private void OnGUI()
	{
		var target = new SerializedObject(this);
		target.Update();
		EditorGUILayout.PropertyField(target.FindProperty(nameof(font)));

		if (GUILayout.Button("Change"))
		{
			font.characterInfo = new CharacterInfo[2];
			font.characterInfo[0].minX = 29;
		}

		target.ApplyModifiedProperties();
	}
}