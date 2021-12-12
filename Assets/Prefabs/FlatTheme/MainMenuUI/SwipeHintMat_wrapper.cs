using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.MainMenuUI
{
    [ExecuteAlways]
	public class SwipeHintMat_wrapper : MonoBehaviour
	{
        Material material;
        
        private void Awake() {
            material = GetComponent<UnityEngine.UI.Image>().material;
        }
        
        [Range(0f, 1f)] public float center, range, add;
        float last_center, last_range, last_add;
        
        private void Update() {
            
            if(last_center != center)
                material.SetFloat("_Center", center);
            if(last_range != range)
                material.SetFloat("_Range", range);
            if(last_add != add)
                material.SetFloat("_Add", add);
            
            last_center = center;
            last_range = range;
            last_add = add;
        }
	}
}
