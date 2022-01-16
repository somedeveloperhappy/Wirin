using UnityEngine;

namespace FlatTheme.Common
{
    public class OnCanvasEnabledAnimPlay : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
    {
        public Animator animator;
        public string inAnimName = "in_anim";

        [ContextMenu("Auto Resolve")]
        public void AutoResolve() => animator = GetComponent<Animator>();
        public void OnCanvasDisable()
        {
            animator.StopPlayback();
            animator.enabled = false;
        }

        public void OnCanvasEnable()
        {
            animator.enabled = true;
            animator.Play(inAnimName);
        }
    }
}