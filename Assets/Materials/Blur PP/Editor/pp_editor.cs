using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class pp_editor : ShaderGUI
{
    enum BlurSamples {
        low     = 2,
        medum   = 6,
        high    = 10,
        ultra   = 20
    };
    
    BlurSamples blurSamples = 0;
    
    
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        var mat = materialEditor.target as Material;
        
        Dictionary<string,MaterialProperty> props = new Dictionary<string, MaterialProperty>();
        
        foreach (var prop in properties) props.Add(prop.name, prop);
        
        GUIStyle style = new GUIStyle(EditorStyles.label);
        
#region blur
             
        style.fontStyle = FontStyle.Bold;
        style.fontSize *= 2;
        
        EditorGUILayout.LabelField("Blur effect", style);
        EditorGUILayout.Space(10);
        
        if(blurSamples == 0) {
            // start of inspector. lets fix it
            if(mat.IsKeywordEnabled("BLUR_ULTRA")) blurSamples = BlurSamples.ultra;
            else if(mat.IsKeywordEnabled("BLUR_HIGH")) blurSamples = BlurSamples.high;
            else if(mat.IsKeywordEnabled("BLUR_MEDIUM")) blurSamples = BlurSamples.medum;
            else blurSamples = BlurSamples.low;
        }
        
        EditorGUI.BeginChangeCheck();
        blurSamples = (BlurSamples)EditorGUILayout.EnumPopup("Quality", blurSamples);
        
        if(EditorGUI.EndChangeCheck()) {
            switch(blurSamples) {
                case BlurSamples.ultra :
                    mat.EnableKeyword("BLUR_ULTRA");
                    break;
                case BlurSamples.high :
                    mat.DisableKeyword("BLUR_ULTRA");
                    mat.EnableKeyword("BLUR_HIGH");
                    break;
                case BlurSamples.medum :
                    mat.DisableKeyword("BLUR_ULTRA");
                    mat.DisableKeyword("BLUR_HIGH");
                    mat.EnableKeyword("BLUR_MEDIUM"); 
                    break;
                case BlurSamples.low :
                    mat.DisableKeyword("BLUR_ULTRA");
                    mat.DisableKeyword("BLUR_HIGH");
                    mat.DisableKeyword("BLUR_MEDIUM"); 
                    break;
            }
        }
        
        props["_blur_intensity"].floatValue = EditorGUILayout.Slider( "Blur Intensity", props["_blur_intensity"].floatValue, 0, 100);
        
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
        
        
        base.OnGUI(materialEditor, properties);
        
    }
}
