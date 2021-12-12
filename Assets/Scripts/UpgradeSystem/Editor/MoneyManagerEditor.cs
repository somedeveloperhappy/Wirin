using System.ComponentModel;
using UnityEngine;
using UnityEditor;
using UpgradeSystem;

[CustomEditor (typeof (MoneyManager))]
public class MoneyManagerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();
		
		var tar = target as MoneyManager;
		
		if(GUILayout.Button("Load"))
		{
			Undo.RecordObject(tar, "money manager changed");
			tar.Coins = uint.Parse(PlayerPrefs.GetString("coins", "0"));
			EditorUtility.SetDirty(serializedObject.targetObject);
		}
		
		if(GUILayout.Button("Save"))
		{
			PlayerPrefs.SetString("coins", tar.Coins.ToString());
		}
	}
}
