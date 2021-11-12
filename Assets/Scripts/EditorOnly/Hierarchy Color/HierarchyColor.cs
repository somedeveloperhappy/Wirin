using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
// using System;
// using System.Linq;

[ExecuteInEditMode]
public class HierarchyColor : MonoBehaviour
{

    private void OnEnable() {

        EditorApplication.hierarchyWindowItemOnGUI += onGUI;

        buttonContent = new GUIContent (string.Empty, (Texture) AssetDatabase.LoadAssetAtPath (@"Assets/Scripts/EditorOnly/Hierarchy Color/eyedropper_icon.png", typeof (Texture)));
        buttonContent.tooltip = "Click on this to open the color editor window";
        labelStyles = new GUIStyle(EditorStyles.label);
        
        EditorApplication.RepaintHierarchyWindow ();

    }

    private void OnDisable() {

        EditorApplication.hierarchyWindowItemOnGUI -= onGUI;

        EditorApplication.RepaintHierarchyWindow ();
    }

    static Color32 gui_normal_background = new Color32 (56, 56, 56, 255);

    [SerializeField]
    public Dictionary_Int_Color col_memory = new Dictionary_Int_Color ();



    [HideInInspector] public Color editingColor_fore;
    GUIContent buttonContent = null;


    [HideInInspector]
    public List<int> instancIDs = new List<int> ();
    int firstItemID = 0;

    GUIStyle labelStyles;
    
    private void onGUI(int instanceID, Rect selectionRect) {

        
        // button
        if (firstItemID == 0) firstItemID = instanceID;
        if (instanceID == firstItemID) {
            var btnRect = selectionRect;
            btnRect.x = btnRect.x + btnRect.width - 40;
            btnRect.width = 30;

            if (GUI.Button (btnRect, buttonContent)) {
                HierarchyColorWindow.hierarchyColor = this;
                HierarchyColorWindow.ShowWindow ();
            }
        }
        
        if (!instancIDs.Contains (instanceID)) instancIDs.Add (instanceID);
        
        GameObject current = (GameObject) EditorUtility.InstanceIDToObject(instanceID);
        if(current == null) return;
        
        if(current.CompareTag("EditorOnly")) {
            
            // if selected, ignore
            if(Selection.Contains(instanceID)) return;
            
            selectionRect.x -= 100;
            selectionRect.width += 120;
            EditorGUI.DrawRect(selectionRect, gui_normal_background);
            selectionRect.x += 100;
            selectionRect.width -= 120;
            
            
            // draw background
            selectionRect.x += 17;
            selectionRect.width -= 17;
            selectionRect.y -= 1;
            
            
            // draw label
            labelStyles.normal.textColor = col_memory.ContainsKey(instanceID) ? col_memory[instanceID] : Color.white;
            labelStyles.fontStyle = FontStyle.Bold;
            EditorGUI.LabelField(selectionRect, current.name, labelStyles);
            labelStyles.fontStyle = FontStyle.Normal;
            
        } else {
            
            // if selected or does not have a color preference, ignore everything else
            if(Selection.Contains(instanceID) || !col_memory.ContainsKey(instanceID)) return;
            
            // draw background
            selectionRect.x += 17;
            selectionRect.width -= 17;
            selectionRect.y -= 1;
            
            // draw label
            labelStyles.normal.textColor = col_memory[instanceID];
            EditorGUI.LabelField(selectionRect, current.name, labelStyles);
            
        }
        


    }
}
