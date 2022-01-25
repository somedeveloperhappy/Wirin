using UnityEngine;

namespace Settings
{
    public class SettingsAcceptance : MonoBehaviour
    {
        public QualitySetterButton[] qualitySettings;

        [ContextMenu("Auto Resolve")]
        public void AutoResolve()
        {
            qualitySettings = GetComponentsInChildren<QualitySetterButton>();
        }

        public void AcceptSettings()
        {
            // find quality
            GlobalSettings.Quality quality = 0;
            foreach (var item in qualitySettings) if (!item.Button.interactable) quality = item.qualityValue;

            // fine theme
            GlobalSettings.Theme theme = 0;
            // ...

            // apply them
            GlobalSettings.SetQuality(quality);
            // GlobalSettings.SetTheme(theme);
        }
    }
}