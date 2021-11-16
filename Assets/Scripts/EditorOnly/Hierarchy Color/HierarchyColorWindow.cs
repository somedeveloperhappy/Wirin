#if UNITY_EDITOR
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

                EditorGUILayout.BeginHorizontal();
                
                if (GUILayout.Button ("Apply", GUILayout.Height(30))) {

                        serializedObject.ApplyModifiedProperties ();

                        // applying to all items selected
                        foreach (var go in Selection.gameObjects)
                                if (HierarchyColor.instancIDs.Contains (HierarchyColor.GetLocalID (go)))
                                        target.col_memory[ HierarchyColor.GetLocalID (go) ] = target.editingColor_fore;

                }

                if (GUILayout.Button ("Remove", GUILayout.Height(30))) {

                        serializedObject.ApplyModifiedProperties ();

                        // removing the selected objects form the dictionary
                        foreach (var go in Selection.gameObjects)
                                if (HierarchyColor.instancIDs.Contains (HierarchyColor.GetLocalID (go)))
                                        target.col_memory.Remove (HierarchyColor.GetLocalID (go));

                }
                
                EditorGUILayout.EndHorizontal();


        }
}
#endif
