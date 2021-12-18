using System.Collections;
using UnityEngine;

public class UpgradeMenuFunctions : MonoBehaviour
{
    private CanvasSystem.CanvasBase m_canvas;

    [System.Serializable]
    public class FadeSettings
    {
        public float fadeInSpeed = 2, fadeOutSpeed = 1;
    }
    public FadeSettings fadeSettings;

    private void Awake()
    {
        m_canvas = GetComponent<CanvasSystem.CanvasBase>();
    }

    public void ShowCanvas()
    {
        Debug.Log( $"showing upgrade canvas" );
        StartCoroutine( FadeIn() );
    }
    public void HideCanvas()
    {
        StartCoroutine( FadeOut() );
    }

    public IEnumerator FadeOut()
    {
        var canvasGroup = GetComponent<CanvasGroup>();

        // fade out
        canvasGroup.alpha = 1;
        do
        {
            canvasGroup.alpha = Mathf.MoveTowards( canvasGroup.alpha, 0, fadeSettings.fadeOutSpeed * Time.unscaledDeltaTime );
            yield return null;
        }
        while (canvasGroup.alpha > 0);

        // disable
        m_canvas.enabled = false;
    }

    public IEnumerator FadeIn()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        m_canvas.enabled = true;

        // fade out
        canvasGroup.alpha = 0;
        do
        {
            canvasGroup.alpha = Mathf.MoveTowards( canvasGroup.alpha, 1, fadeSettings.fadeInSpeed * Time.unscaledDeltaTime );
            yield return null;
        }
        while (canvasGroup.alpha < 1);
    }


}
