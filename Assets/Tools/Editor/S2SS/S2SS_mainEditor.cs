using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class S2SS_mainEditor : EditorWindow
{
	[SerializeField] private int columns, rows;

	[SerializeField] private bool deleteTexturesAfterwards = true;
	[SerializeField] private int margin = 1;

	[SerializeField] private string slicingSprite;
	[SerializeField] private int spritesInOneRow = 2;
	[SerializeField] private Texture[] textures;

	[MenuItem("Wirin/S2SS_main")]
	private static void ShowWindow()
	{
		var window = GetWindow<S2SS_mainEditor>();
		window.titleContent = new GUIContent("S2SS_main");
		window.Show();
	}

	private void OnGUI()
	{
		var target = new SerializedObject(this);

		EditorGUILayout.Space(10);

		var style = new GUIStyle(EditorStyles.boldLabel) {fontSize = 24};
		EditorGUILayout.LabelField("S2SS", style);

		EditorGUILayout.Space(10);

		if (GUILayout.Button("Clear texture list", GUILayout.Height(30)))
			target.FindProperty(nameof(textures)).arraySize = 0;

		EditorGUILayout.PropertyField(target.FindProperty(nameof(textures)));


		if (target.FindProperty(nameof(textures)).arraySize > 1)
		{
			if (textures[0] == null)
			{
				textures = new Texture[0];
				return;
			}

			EditorGUILayout.IntSlider(target.FindProperty(nameof(spritesInOneRow)), 0, textures.Length);
			EditorGUILayout.IntSlider(target.FindProperty(nameof(margin)), 0, 10);

			columns = spritesInOneRow;
			rows = Mathf.CeilToInt((float) textures.Length / spritesInOneRow);

			EditorGUILayout.LabelField("Count : " + textures.Length);
			EditorGUILayout.LabelField(
				$"each sequence\t: width : {textures[0].width} px\theight : {textures[0].height} px");
			EditorGUILayout.LabelField(
				$"result\t\t: width : {(textures[0].width + margin) * spritesInOneRow} px\theight : {(textures[0].height + margin) * Mathf.CeilToInt((float) textures.Length / spritesInOneRow)} px");
			EditorGUILayout.LabelField($"columns : {columns}\trows : {rows}");


			EditorGUILayout.Space(10);

			for (var i = 0; i < textures.Length; i++)
			{
				var ti = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(textures[i])) as TextureImporter;

				if (!ti.isReadable)
				{
					var oldcol = GUI.color;
					GUI.color = Color.red;

					if (GUILayout.Button("Set all selected textures readable", GUILayout.Height(30)))
					{
						for (i = 0; i < textures.Length; i++)
						{
							var path = AssetDatabase.GetAssetPath(textures[i]);
							(AssetImporter.GetAtPath(path) as TextureImporter).isReadable = true;
							AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
						}

						GUI.color = oldcol;
						goto after_generate;
					}

					GUI.color = oldcol;
					goto after_generate;
				}
			}

			EditorGUILayout.PropertyField(target.FindProperty(nameof(deleteTexturesAfterwards)));

			if (GUILayout.Button("Generate", GUILayout.Height(30)))
			{
				if (textures.Length < 1)
				{
					Debug.LogError("You have to assign textures first!");
					goto after_generate;
				}

				var path = EditorUtility.SaveFilePanelInProject("new spritesheet file",
					Regex.Replace(textures[0].name, @"[\d-]", string.Empty), "png",
					"Select where to save the spritesheet");

				// generate spritesheet
				S2SS_main.Generate(textures, spritesInOneRow, margin, path);

				// slice spritesheet
				S2SS_main.SlideSprite(
					path.Substring(path.IndexOf("Assets")),
					columns,
					rows,
					textures[0],
					margin);

				if (deleteTexturesAfterwards)
					for (var i = 0; i < textures.Length; i++)
						AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(textures[i]));
			}
		}

		after_generate:


		target.ApplyModifiedProperties();
	}
}