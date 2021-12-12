using SimpleScripts;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransformCopyCat))]
public class TransformCopyCatEditor : Editor
{
	public override void OnInspectorGUI()
	{
		var oldgui = GUI.enabled;
		GUI.enabled = false;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
		GUI.enabled = oldgui;

		serializedObject.Update();

		var tar = target as TransformCopyCat;


		EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));

		if (tar.target == null)
		{
			EditorGUILayout.HelpBox("The target should not be null!", MessageType.Error);
			GUI.color = Color.red;
		}

		EditorGUILayout.PropertyField(serializedObject.FindProperty("position"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("rotation"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("scale"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("applyTime"));

		serializedObject.ApplyModifiedProperties();
	}
}

[CustomPropertyDrawer(typeof(TransformCopyCat.CopyState))]
public class CopyStateEditor : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{

		label = EditorGUI.BeginProperty(position, label, property);
		EditorGUI.BeginChangeCheck();

		// enabled toggle
		position.width = 20;
		var enabled_prop = property.FindPropertyRelative("enabled");
		enabled_prop.boolValue = EditorGUI.Toggle(position, enabled_prop.boolValue);

		// label
		position.x += position.width;
		position.width = EditorGUIUtility.labelWidth - 20;
		EditorGUI.LabelField(position, label);


		if (enabled_prop.boolValue)
		{
			position.width /= 2f;
			position.x += position.width;

			// space enum
			EditorGUI.PropertyField(position, property.FindPropertyRelative("space"), GUIContent.none);

			position.x += position.width;
			position.width = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 50) / 6;

			// x label
			EditorGUI.LabelField(position, "x");
			// x toggle
			position.x += position.width;
			var x_prop = property.FindPropertyRelative("x");
			x_prop.boolValue = EditorGUI.Toggle(position, x_prop.boolValue);

			// y label
			position.x += position.width;
			EditorGUI.LabelField(position, "y");
			// y toggle
			position.x += position.width;
			var y_prop = property.FindPropertyRelative("y");
			y_prop.boolValue = EditorGUI.Toggle(position, y_prop.boolValue);

			// z label
			position.x += position.width;
			EditorGUI.LabelField(position, "z");
			// z toggle
			position.x += position.width;
			var z_prop = property.FindPropertyRelative("z");
			z_prop.boolValue = EditorGUI.Toggle(position, z_prop.boolValue);
		}

		if (EditorGUI.EndChangeCheck())
			// save
			property.serializedObject.ApplyModifiedProperties();
		EditorGUI.EndProperty();
	}
}