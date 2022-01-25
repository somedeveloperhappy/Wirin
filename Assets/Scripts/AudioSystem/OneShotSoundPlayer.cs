using UnityEngine;

public class OneShotSoundPlayer : MonoBehaviour
{
        public AudioClip clip;
        public UnityEngine.UI.Button button;
        [Range(0f, 1f)] public float volume = 1;

        public void PlaySound()
        {
                References.menu_sfx.Play(clip);
        }

        [ContextMenu("Auto resolve")]
        public void AutoResolve()
        {
                button = GetComponent<UnityEngine.UI.Button>();
                if (!button)
                        button = GetComponentInChildren<UnityEngine.UI.Button>();
#if UNITY_EDITOR
                UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(button.onClick, PlaySound);
#endif
        }
}
