using UnityEngine;

namespace FlatTheme
{
    [ExecuteAlways]
    public class FlatBackgroundPP : MonoBehaviour
    {
        public Material postProcessMaterial;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, postProcessMaterial);
        }

        public void SetFogColor(Color color)
        {
            postProcessMaterial.SetColor("_FogColor", color);
        }

        public void SetFogIntensity(float intensity)
        {
            postProcessMaterial.SetFloat("_FogIntensity", intensity);
        }

        public void SetSaturation(float saturation)
        {
            postProcessMaterial.SetFloat("_Saturation", saturation);
        }
    }
}
