using UnityEngine;
using UnityEngine.Serialization;

[ExecuteAlways]
public class PostPro : MonoBehaviour
{


    private new Camera camera;

    [FormerlySerializedAs("blur_effect")] public Material pp_mat;

    private RenderTexture rt;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }


    public void SetChromIntensity(float value)
    {
        pp_mat.SetFloat("_chromatic_intensity", value);
    }

    /// <summary>
    ///     sets lens distortion value. value better be between -1 and 1
    /// </summary>
    /// <param name="value"></param>
    public void SetLensDistortion(float value)
    {
        pp_mat.SetFloat("_lens_distortion", value);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        Graphics.Blit(src, null, pp_mat);
    }
}