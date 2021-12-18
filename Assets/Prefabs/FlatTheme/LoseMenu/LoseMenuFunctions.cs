using System;
using System.Collections;
using UnityEngine;

namespace FlatTheme.LoseMenu
{
    public class LoseMenuFunctions : MonoBehaviour
    {
        public UnityEngine.UI.GraphicRaycaster graphicRaycaster;
        public CanvasSystem.CanvasBase canvasBase;
        public CanvasGroup canvasGroup;

        [System.Serializable]
        public class FadeOutSettings
        {
            public float fadeOutSpeed = 1;
        }
        public FadeOutSettings fadeOutSettings;

        [ContextMenu( "Auto Resolve" )]
        public void AutoResolve()
        {
            graphicRaycaster = GetComponent<UnityEngine.UI.GraphicRaycaster>();
            canvasBase = GetComponent<CanvasSystem.CanvasBase>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ExitToMainMenu()
        {
            StartCoroutine( ExitToMenuAsync() );
        }

        private IEnumerator ExitToMenuAsync()
        {
            // end gameplay mechanichs
            References.gameController.EndGameplay( timeScaleTo0: true );

            // open main menu in background
            References.gameController.DisableAllCanvasesExceptFor(
                new CanvasSystem.CanvasBase[] { canvasBase, References.gameController.mainMenuCanvas }
            );

            // disable functionlities of the canvas
            graphicRaycaster.enabled = false;

            // hide 
            do
            {
                canvasGroup.alpha -= Time.unscaledDeltaTime * fadeOutSettings.fadeOutSpeed;
                yield return null;
            } while (canvasGroup.alpha > 0);

            // set absolute values
            canvasGroup.alpha = 0;

            // completely disbale this canvas
            canvasBase.enabled = false;

            // things back to normal
            graphicRaycaster.enabled = true;

            // open main menu
            References.gameController.OpenMainMenu();
            Debug.Log( "Finished exitToMenuAsync shit" );

        }
    }
}