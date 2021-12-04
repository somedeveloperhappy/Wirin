#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class HierarchyColorWindow : EditorWindow
{
	public static bool isOpen;

	public static HierarchyColor hierarchyColor;


	private HierarchyColor target;

	// [MenuItem ("Wirin/HierarchyColorWindow")]
	public static void ShowWindow()
	{
		var window = GetWindow<HierarchyColorWindow>();
		window.titleContent = new GUIContent("HierarchyColorWindow");
		window.Show();
	}

	private void OnEnable()
	{
		isOpen = true;
	}

	private void OnDisable()
	{
		isOpen = false;
	}

	private void OnGUI()
	{
		if (hierarchyColor == null)
		{
			Close();
			return;
		}

		var serializedObject = new SerializedObject(hierarchyColor);
		target = serializedObject.targetObject as HierarchyColor;

		EditorGUILayout.LabelField("Selected : " + target.name);

		EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(target.editingColor_fore)));

		serializedObject.ApplyModifiedProperties();

		EditorGUILayout.BeginHorizontal();

		if (GUILayout.Button("Apply", GUILayout.Height(30)))
		{
			serializedObject.ApplyModifiedProperties();

			// applying to all items selected
			foreach (var go in Selection.gameObjects)
				if (HierarchyColor.instancIDs.Contains(HierarchyColor.GetLocalID(go)))
					target.col_memory[HierarchyColor.GetLocalID(go)] = target.editingColor_fore;
		}

		if (GUILayout.Button("Apply recursive", GUILayout.Height(30)))
		{
			serializedObject.ApplyModifiedProperties();

			// applying to selected items
			foreach (var go in Selection.gameObjects)
				if (HierarchyColor.instancIDs.Contains(HierarchyColor.GetLocalID(go)))
				{
					// apply to selected
					target.col_memory[HierarchyColor.GetLocalID(go)] = target.editingColor_fore;
					// appy to childs of selected
					ApplyRecursive(go);
				}
		}

		if (GUILayout.Button("Remove", GUILayout.Height(30)))
		{
			serializedObject.ApplyModifiedProperties();

			// removing the selected objects form the dictionary
			foreach (var go in Selection.gameObjects)
				if (HierarchyColor.instancIDs.Contains(HierarchyColor.GetLocalID(go)))
					target.col_memory.Remove(HierarchyColor.GetLocalID(go));
		}

		EditorGUILayout.EndHorizontal();
	}

	private void ApplyRecursive(GameObject go)
	{
		foreach (Transform child in go.transform)
		{
			var id = HierarchyColor.GetLocalID(child.gameObject);

			hierarchyColor.col_memory[id] = target.editingColor_fore;
			// recurse now
			ApplyRecursive(child.gameObject);
		}
	}
}
#endif