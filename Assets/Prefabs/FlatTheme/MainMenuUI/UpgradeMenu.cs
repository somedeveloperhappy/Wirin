using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenu : MonoBehaviour
{
	private CanvasSystem.CanvasBase m_canvas;

	private void Awake()
	{
		m_canvas = GetComponent<CanvasSystem.CanvasBase> ();
	}

	public void ShowCanvas(float fadeSpeed)
	{
		Debug.Log($"showing upgrade canvas");
		if (fadeSpeed <= 0)
			m_canvas.enabled = true;
		else
			StartCoroutine(FadeIn (fadeSpeed));
	}
	public void HideCanvas(float fadeSpeed)
	{
		if (fadeSpeed <= 0)
			m_canvas.enabled = false;
		else
			StartCoroutine(FadeOut (fadeSpeed));
	}

	public IEnumerator FadeOut(float fadeSpeed)
	{
		var canvasGroup = GetComponent<CanvasGroup> ();

		// fade out
        canvasGroup.alpha = 1;
		do
		{
			canvasGroup.alpha -= fadeSpeed * Time.unscaledDeltaTime;
			yield return null;
		}
		while (canvasGroup.alpha > 0);
        canvasGroup.alpha = 0;

        Debug.Log($"finished");
		// disable
		m_canvas.enabled = false;
	}

	public IEnumerator FadeIn(float fadeSpeed)
	{
		var canvasGroup = GetComponent<CanvasGroup> ();

		// enable
		m_canvas.enabled = true;

		// fade out
        canvasGroup.alpha = 0;
		do
		{
			canvasGroup.alpha += fadeSpeed * Time.unscaledDeltaTime;
			yield return null;
		}
		while (canvasGroup.alpha < 1);
        canvasGroup.alpha = 1;
	}
    
    
}
