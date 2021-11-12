using UnityEngine;
using UnityEditor;

public class HierarchyColorWindow : EditorWindow
{

    // [MenuItem ("Wirin/HierarchyColorWindow")]
    public static void ShowWindow() {
        var window = GetWindow<HierarchyColorWindow> ();
        window.titleContent = new GUIContent ("HierarchyColorWindow");
        window.Show ();
    }

    static public HierarchyColor hierarchyColor;

    private void OnGUI() {

        if (hierarchyColor == null) {
            Close ();
            return;
        }

        var serializedObject = new SerializedObject (hierarchyColor);
        var target = serializedObject.targetObject as HierarchyColor;

        EditorGUILayout.LabelField ("Selected : " + target.name);

        EditorGUILayout.PropertyField (serializedObject.FindProperty (nameof (target.editingColor_fore)));

        serializedObject.ApplyModifiedProperties ();

        if (GUILayout.Button ("Apply on selected")) {

            serializedObject.ApplyModifiedProperties ();

            // applying to all items selected
            foreach (var go in Selection.gameObjects)
                if (hierarchyColor.instancIDs.Contains (go.GetInstanceID ()))
                    target.col_memory[ go.GetInstanceID () ] = target.editingColor_fore;

        }


    }
}
