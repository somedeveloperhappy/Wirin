using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FlatTheme.MainMenuUI
{
	public class MainMenuCanvasFunctions : MonoBehaviour
	{
		public CanvasSystem.CanvasBase canvasBase;
		public UnityEngine.UI.GraphicRaycaster graphicRaycaster;
		public CanvasGroup canvasGroup;

		[System.Serializable]
		public class FadeSettings
		{
			public float fadeSpeed = 5;
		}
		public FadeSettings fadeSettings;

		public void HideCanvasAndStartGame()
		{
			StartCoroutine (HideCanvasAndStartGameAwait ());
		}
		private IEnumerator HideCanvasAndStartGameAwait()
		{
			// disable further inputs
			graphicRaycaster.enabled = false;

			// fade out 
			do
			{
				canvasGroup.alpha -= fadeSettings.fadeSpeed * Time.unscaledDeltaTime;
				yield return null;
			}
			while (canvasGroup.alpha > 0);
			canvasGroup.alpha = 0;

			// restore defaults
			graphicRaycaster.enabled = true;

			// disable fully
			canvasBase.enabled = false;

			// start game
			References.gameController.StartGame ();
		}
	}

}
