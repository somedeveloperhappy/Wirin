using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace FlatTheme.MainMenuUI.UpgradeItems
{
	[CustomEditor (typeof (HealthUpgradeButton))]
	public class HealthUpgradeButtonEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI ();
			if (GUILayout.Button ("Auto Resolve"))
			{
				var tar = target as HealthUpgradeButton;
				
				var btn = tar.GetComponentInChildren<Button>();
				if(btn != null) tar.button = btn;
				
				var txts = tar.GetComponentsInChildren<Text>();
				
				void ResolveTxt(string name)
				{
					(Text text, int point) bestmatch = (null, 0);
					foreach(var t in txts)
					{
						if(bestmatch.point == 0)
						{
							bestmatch.text = t;
							bestmatch.point = SimpleScripts.HandyFuncs.str_eq_point(name, t.name);
							continue;
						}
						
						var p = SimpleScripts.HandyFuncs.str_eq_point(name, t.name);
						if(p > bestmatch.point)
						{
							bestmatch.text = t;
							bestmatch.point = SimpleScripts.HandyFuncs.str_eq_point(name, t.name);
						}
					}
					if(bestmatch.text != null)
					{
						serializedObject.FindProperty(name).objectReferenceValue = bestmatch.text;
					}
				}
				
				ResolveTxt(nameof(tar.costText));
				ResolveTxt(nameof(tar.levelText));
				
				EditorUtility.SetDirty(serializedObject.targetObject);
				serializedObject.ApplyModifiedProperties();
				serializedObject.Update();
				
			}
		}
	}
}
