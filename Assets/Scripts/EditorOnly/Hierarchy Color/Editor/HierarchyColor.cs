using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[ExecuteInEditMode]
public class HierarchyColor : MonoBehaviour
{

        public Texture2D tex;

        private void OnEnable() {

                Debug.Log ($"on enable");
                EditorApplication.hierarchyWindowItemOnGUI += onGUI;

                tex = (Texture2D) (Texture) AssetDatabase.LoadAssetAtPath (@"Assets/Scripts/EditorOnly/Hierarchy Color/eyedropper_icon.png", typeof (Texture));

                if (!EditorGUIUtility.isProSkin) {
                        Debug.Log ($"setting it black...");
                        // set colors black
                        var cols = tex.GetPixels ();
                        for (int i = 0; i < cols.Length; i++) {
                                cols[ i ].r = cols[ i ].g = cols[ i ].b = 0;
                        }
                        tex.SetPixels (cols);
                        tex.Apply ();
                }

                buttonContent = new GUIContent (string.Empty, tex);
                buttonContent.tooltip = "Click on this to open the color editor window";

                labelStyles = null;

                EditorApplication.RepaintHierarchyWindow ();

        }

        private void OnDisable() {

                EditorApplication.hierarchyWindowItemOnGUI -= onGUI;

                EditorApplication.RepaintHierarchyWindow ();
        }

        static Color32 gui_normal_background = new Color32 (56, 56, 56, 255);
        static Color32 gui_normal_background_light = new Color32 (200, 200, 200, 255);

        [SerializeField]
        public Dictionary_Int_Color col_memory = new Dictionary_Int_Color ();



        [HideInInspector] public Color editingColor_fore;
        GUIContent buttonContent = null;


        static public List<int> instancIDs = new List<int> ();
        int firstItemID = 0;

        GUIStyle labelStyles;

        private void onGUI(int _instanceID, Rect selectionRect) {



                if (labelStyles == null) labelStyles = new GUIStyle (EditorStyles.label);


                // button
                if (firstItemID == 0) firstItemID = _instanceID;
                if (_instanceID == firstItemID) {
                        var btnRect = selectionRect;
                        btnRect.x = btnRect.x + btnRect.width - 40;
                        btnRect.width = 30;

                        if (GUI.Button (btnRect, buttonContent)) {
                                HierarchyColorWindow.hierarchyColor = this;
                                HierarchyColorWindow.ShowWindow ();
                        }
                }

                GameObject current = (GameObject) EditorUtility.InstanceIDToObject (_instanceID);
                if (current == null) return;
                int instanceID = GetLocalID (current);

                if (!instancIDs.Contains (instanceID)) instancIDs.Add (instanceID);

                if (current.CompareTag ("EditorOnly")) {

                        // if selected, ignore
                        if (Selection.Contains (_instanceID)) return;





                        // draw label
                        if (col_memory.ContainsKey (instanceID)) {

                                float x = selectionRect.x;

                                selectionRect.x += 17;
                                // selectionRect.width -= 17;
                                selectionRect.y += -1;

                                // draw on top of the current label
                                labelStyles.normal.textColor = Color.black;
                                EditorGUI.LabelField (selectionRect, current.name, labelStyles);

                                selectionRect.x += 1;
                                selectionRect.y += -1;

                                labelStyles.normal.textColor = col_memory[ instanceID ];
                                EditorGUI.LabelField (selectionRect, current.name, labelStyles);

                                selectionRect.x = x;

                        }

                        // editor only notice
                        selectionRect.x -= 40;
                        selectionRect.width = 30;
                        labelStyles.fontStyle = FontStyle.Italic;
                        EditorGUI.LabelField (selectionRect, "X", labelStyles);
                        labelStyles.fontStyle = FontStyle.Normal;



                } else {

                        // if selected or does not have a color preference, ignore everything else
                        if (Selection.Contains (_instanceID) || !col_memory.ContainsKey (instanceID)) return;

                        selectionRect.x += 17;
                        // selectionRect.width -= 17;
                        selectionRect.y += -1;

                        // draw on top of the current label
                        labelStyles.normal.textColor = Color.black;
                        EditorGUI.LabelField (selectionRect, current.name, labelStyles);

                        selectionRect.x += 1;
                        selectionRect.y += -1;

                        // draw label
                        labelStyles.normal.textColor = col_memory[ instanceID ];
                        EditorGUI.LabelField (selectionRect, current.name, labelStyles);

                }
        }

        static public int GetLocalID(GameObject go) {
                PropertyInfo inspectorModeInfo = typeof (SerializedObject).GetProperty ("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

                SerializedObject serializedObject = new SerializedObject (go);
                inspectorModeInfo.SetValue (serializedObject, InspectorMode.Debug, null);

                SerializedProperty localIdProp = serializedObject.FindProperty ("m_LocalIdentfierInFile");   //note the misspelling!

                return localIdProp.intValue;
        }
}
