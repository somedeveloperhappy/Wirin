#if UNITY_EDITOR
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class HierarchyColor : MonoBehaviour
{

	private static Color32 gui_normal_background = new Color32(56, 56, 56, 255);
	private static Color32 gui_normal_background_light = new Color32(200, 200, 200, 255);


	public static List<int> instancIDs = new List<int>();
	private GUIContent buttonContent;

	[SerializeField] public Dictionary_Int_Color col_memory = new Dictionary_Int_Color();


	[HideInInspector] public Color editingColor_fore;
	private int firstItemID;

	private GUIStyle labelStyles;
	public Texture2D tex;

	private void OnEnable()
	{
		// Debug.Log ($"on enable");
		EditorApplication.hierarchyWindowItemOnGUI += onGUI;

		tex = (Texture2D) (Texture) AssetDatabase.LoadAssetAtPath(
			@"Assets/Tools/Hierarchy Color/eyedropper_icon.png", typeof(Texture));

		if (!EditorGUIUtility.isProSkin)
		{
			// set colors black
			var cols = tex.GetPixels();
			for (var i = 0; i < cols.Length; i++) cols[i].r = cols[i].g = cols[i].b = 0;

			tex.SetPixels(cols);
			tex.Apply();
		}

		buttonContent = new GUIContent(string.Empty, tex);
		buttonContent.tooltip = "Click on this to open the color editor window";

		labelStyles = null;

		EditorApplication.RepaintHierarchyWindow();
	}

	private void OnDisable()
	{
		EditorApplication.hierarchyWindowItemOnGUI -= onGUI;

		EditorApplication.RepaintHierarchyWindow();
	}

	private void onGUI(int _instanceID, Rect selectionRect)
	{
		if (labelStyles == null) labelStyles = new GUIStyle(EditorStyles.label);


		// button
		if (firstItemID == 0) firstItemID = _instanceID;
		if (!HierarchyColorWindow.isOpen && _instanceID == firstItemID)
		{
			var btnRect = selectionRect;
			btnRect.x = btnRect.x + btnRect.width - 40;
			btnRect.width = 30;

			if (GUI.Button(btnRect, buttonContent))
			{
				HierarchyColorWindow.hierarchyColor = this;
				HierarchyColorWindow.ShowWindow();
			}

			return;
		}

		var current = (GameObject) EditorUtility.InstanceIDToObject(_instanceID);
		if (current == null) return;
		var instanceID = GetLocalID(current);

		if (!instancIDs.Contains(instanceID)) instancIDs.Add(instanceID);


		// draw label
		if (col_memory.ContainsKey(instanceID))
		{
			var initRect = selectionRect;

			// if selected, ignore
			if (Selection.Contains(_instanceID)) goto afterLabel;


			var x = selectionRect.x;

			selectionRect.x += 17;
			// selectionRect.width -= 17;
			selectionRect.y += -1;

			Color txtCol;

			if (current.activeInHierarchy)
			{
				txtCol = Color.black;
				labelStyles.normal.textColor = txtCol;

				// black behind ( shadow )
				EditorGUI.LabelField(selectionRect, current.name, labelStyles);
			}

			selectionRect.x += 1;
			selectionRect.y += -1;

			txtCol = col_memory[instanceID];
			if (!current.activeInHierarchy) txtCol.a = 0.4f; // fade if not enabled
			labelStyles.normal.textColor = txtCol;

			// main label
			EditorGUI.LabelField(selectionRect, current.name, labelStyles);

			selectionRect.x = x;

			afterLabel:

			// draw copy-color button if color widow is open
			if (HierarchyColorWindow.isOpen)
			{
				var btnRect = initRect;
				btnRect.x = btnRect.x + btnRect.width - 10;
				btnRect.width = 30;

				if (GUI.Button(btnRect, buttonContent)) // copy the color to color picker
					editingColor_fore = col_memory[instanceID];
			}
		}
		else
		{
			labelStyles.normal.textColor = Color.white;
		}

		if (current.CompareTag("EditorOnly"))
		{
			// editor only notice
			selectionRect.x -= 40;
			selectionRect.width = 30;
			labelStyles.fontStyle = FontStyle.Italic;
			EditorGUI.LabelField(selectionRect, "X", labelStyles);
			labelStyles.fontStyle = FontStyle.Normal;
		}
	}

	public static int GetLocalID(GameObject go)
	{
		var inspectorModeInfo =
			typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

		var serializedObject = new SerializedObject(go);
		inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

		var
			localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile"); //note the misspelling!

		return localIdProp.intValue;
	}


	[ContextMenu("Optimize")]
	public void Optimize()
	{
		var keys = new int[col_memory.Count];
		var i = 0;
		foreach (var mem in col_memory) keys[i++] = mem.Key;

		foreach (var key in keys)
			if (col_memory[key] == Color.white || col_memory[key] == Color.black)
				col_memory.Remove(key);
	}
}
#endif
