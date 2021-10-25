using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class pp_editor : ShaderGUI
{
    enum Samples {
        low     = 2,
        medum   = 6,
        high    = 10,
        ultra   = 20
    };
    
    Samples samples = 0;
    
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var mat = materialEditor.target as Material;
        
        Dictionary<string,MaterialProperty> props = new Dictionary<string, MaterialProperty>();
        
        foreach (var prop in properties) props.Add(prop.name, prop);
        
        GUIStyle style = new GUIStyle(EditorStyles.label);
        
        if(samples == 0) {
            // start of inspector. lets fix it
            if(mat.IsKeywordEnabled("ULTRA")) samples = Samples.ultra;
            else if(mat.IsKeywordEnabled("HIGH")) samples = Samples.high;
            else if(mat.IsKeywordEnabled("MEDIUM")) samples = Samples.medum;
            else if(mat.IsKeywordEnabled("LOW")) samples = Samples.low;
            
            Debug.Log($"{string.Join(", ", mat.shaderKeywords)}");
        }
        
        EditorGUI.BeginChangeCheck();
        samples = (Samples)EditorGUILayout.EnumPopup("Quality", samples);
        
        if(EditorGUI.EndChangeCheck()) {
            switch(samples) {
                case Samples.ultra :
                    mat.EnableKeyword("ULTRA");
                    mat.DisableKeyword("HIGH");
                    mat.DisableKeyword("MEDIUM");
                    mat.DisableKeyword("LOW");
                    break;
                case Samples.high :
                    mat.DisableKeyword("ULTRA");
                    mat.EnableKeyword("HIGH");
                    mat.DisableKeyword("MEDIUM");
                    mat.DisableKeyword("LOW");
                    break;
                case Samples.medum :
                    mat.DisableKeyword("ULTRA");
                    mat.DisableKeyword("HIGH");
                    mat.EnableKeyword("MEDIUM");
                    mat.DisableKeyword("LOW");
                    break;
                case Samples.low :
                    mat.DisableKeyword("ULTRA");
                    mat.DisableKeyword("HIGH");
                    mat.DisableKeyword("MEDIUM");
                    mat.EnableKeyword("LOW");
                    break;
            }
        }

        
#region blur
             
        style.fontStyle = FontStyle.Bold;
        style.fontSize *= 2;
        
        EditorGUILayout.LabelField("Blur effect", style);
        EditorGUILayout.Space(10);        
        
        props["_blur_intensity"].floatValue = EditorGUILayout.Slider( "Blur Intensity", props["_blur_intensity"].floatValue, 0, 1);
        
        EditorGUI.BeginChangeCheck();
        props["_blurGuassian"].floatValue = EditorGUILayout.Toggle("Is Guassian", props["_blurGuassian"].floatValue==1) ? 1 : 0;
        if(EditorGUI.EndChangeCheck()) {
            if(props["_blurGuassian"].floatValue == 1) 
                mat.EnableKeyword("BLUR_GUASSIAN");
            else
                mat.DisableKeyword("BLUR_GUASSIAN");
        }
        
        EditorGUI.BeginChangeCheck();
        props["_blur_mask"].textureValue = (Texture)EditorGUILayout.ObjectField("Blur Mask", props["_blur_mask"].textureValue, typeof(Texture), false);
        if(EditorGUI.EndChangeCheck()) {
            // check for texture being null
            if(props["_blur_mask"].textureValue == null) mat.DisableKeyword("BLUR_MASK"); else mat.EnableKeyword("BLUR_MASK");
            
        }
        
#endregion
        
        foreach (var kw in mat.shaderKeywords) EditorGUILayout.LabelField(kw + " : "+mat.IsKeywordEnabled(kw));
        
        base.OnGUI(materialEditor, properties);
        
    }
}
