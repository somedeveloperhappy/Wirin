using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PressFX;

public class CameraEffectsOnPress : MonoBehaviour, IOnPressFx
{
	[System.Serializable]
	public class ShaderEffect
	{
		public MinMax chromaticIntensity, lensDistortion;
	}

	[System.Serializable]
	public class CameraEffect
	{
		public MinMax cameraSize;
	}

	public ShaderEffect shaderEffect;
	public CameraEffect cameraEffect;

	private void Start()
	{
		this.Initialize ();
	}

	public void Apply(float normalizedT)
	{
		References.postPro.SetChromIntensity (
            Mathf.Lerp (shaderEffect.chromaticIntensity.min, shaderEffect.chromaticIntensity.max, normalizedT));
            
		References.postPro.SetLensDistortion (
            Mathf.Lerp (shaderEffect.lensDistortion.min, shaderEffect.lensDistortion.max, normalizedT));
            
		References.currentCamera.orthographicSize = Mathf.Lerp (
            cameraEffect.cameraSize.min, cameraEffect.cameraSize.max, normalizedT);
	}

	public void Initialize() => this.DefaultInitialize ();
}
