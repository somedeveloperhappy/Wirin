using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class pp_editor : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var mat = materialEditor.target as Material;
        
        Dictionary<string,MaterialProperty> props = new Dictionary<string, MaterialProperty>();
        
        foreach (var prop in properties) props.Add(prop.name, prop);
        
        props["_blur_intensity"].floatValue = EditorGUILayout.IntSlider( "Blur Intensity", (int)props["_blur_intensity"].floatValue, 0, 100);
        props["_blurCombiner"].floatValue = EditorGUILayout.Slider( "Blur Intensity", props["_blurCombiner"].floatValue, 0, 1);
        
        EditorGUI.BeginChangeCheck();
        props["_blurGuassian"].floatValue = EditorGUILayout.Toggle("Is Guassian", props["_blurGuassian"].floatValue==1) ? 1 : 0;
        if(EditorGUI.EndChangeCheck()) {
            if(props["_blurGuassian"].floatValue == 1) 
                mat.EnableKeyword("BLUR_GUASSIAN");
            else
                mat.DisableKeyword("BLUR_GUASSIAN");
        }
        
        base.OnGUI(materialEditor, properties);
        
    }
}
