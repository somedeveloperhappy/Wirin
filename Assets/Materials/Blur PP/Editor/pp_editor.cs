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
        
        
        materialEditor.ShaderProperty(props["_Samples"], "Quality");
        
#region blur
             
        materialEditor.ShaderProperty(props["_blurEffect"], "Blur");
        
        if(mat.IsKeywordEnabled("BLUR"))
        {
            style.fontStyle = FontStyle.Bold;
            style.fontSize *= 2;
            
            EditorGUILayout.LabelField("Blur effect", style);
            EditorGUILayout.Space(10);
            
            // x and y are for pass 1 direcition. z and w are for pass 2 direction
            var blur_dir1 = new Vector2(props["_blur_1_dir_x"].floatValue, props["_blur_1_dir_y"].floatValue);
            var blur_dir2 = new Vector2(props["_blur_2_dir_x"].floatValue, props["_blur_2_dir_y"].floatValue);
            EditorGUI.BeginChangeCheck();
            // pass 1 dir
            blur_dir1.x = EditorGUILayout.Slider("blur direction : pass 1 X", blur_dir1.x, -1, 1);
            blur_dir1.y = EditorGUILayout.Slider("blur direction : pass 1 Y", blur_dir1.y, -1, 1);
            blur_dir2.x = EditorGUILayout.Slider("blur direction : pass 2 X", blur_dir2.x, -1, 1);
            blur_dir2.y = EditorGUILayout.Slider("blur direction : pass 2 Y", blur_dir2.y, -1, 1);
            if(EditorGUI.EndChangeCheck()) {
                props["_blur_1_dir_x"].floatValue = blur_dir1.x;
                props["_blur_1_dir_y"].floatValue = blur_dir1.y;
                props["_blur_2_dir_x"].floatValue = blur_dir2.x;
                props["_blur_2_dir_y"].floatValue = blur_dir2.y;
            }
            
            
            props["_blur_intensity"].floatValue = EditorGUILayout.Slider( "Blur Intensity", props["_blur_intensity"].floatValue, 0, 1);
            
            materialEditor.ShaderProperty(props["_blur"], "Blur Type");
            
            EditorGUI.BeginChangeCheck();
            materialEditor.ShaderProperty(props["_blur_mask"], "Blur Mask");
            if(EditorGUI.EndChangeCheck()) 
                // check for texture being null
                if(props["_blur_mask"].textureValue == null) mat.DisableKeyword("BLUR_MASK"); else mat.EnableKeyword("BLUR_MASK");
        }
        
#endregion

#region chromatic aberration
        materialEditor.ShaderProperty(props["_chromEffect"], "Chromatic Aberration");
        
        if(mat.IsKeywordEnabled("CHROM"))
        {
            props["_chrom_intensity"].floatValue = EditorGUILayout.Slider(props["_chrom_intensity"].floatValue, 0, 1);
        }

#endregion
        
        if(GUILayout.Button("reset keywords")) {
            foreach(var kw in mat.shaderKeywords) mat.DisableKeyword(kw);
        }
        foreach (var kw in mat.shaderKeywords) EditorGUILayout.LabelField(kw + " : "+mat.IsKeywordEnabled(kw));
        
        base.OnGUI(materialEditor, properties);
        
    }
}
