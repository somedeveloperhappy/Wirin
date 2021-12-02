using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PostPro : MonoBehaviour
{
    [UnityEngine.Serialization.FormerlySerializedAs("blur_effect")]
    public Material pp_mat;


    new Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();
    }

    RenderTexture rt;


    public void SetChromIntensity(float value) {
        pp_mat.SetFloat("_chromatic_intensity", value);
    }

    /// <summary>
    /// sets lens distortion value. value better be between -1 and 1
    /// </summary>
    /// <param name="value"></param>
    public void SetLensDistortion(float value) {
        pp_mat.SetFloat("_lens_distortion", value);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, null, pp_mat);
    }
}
