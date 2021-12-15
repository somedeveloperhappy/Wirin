using UnityEngine;

public class CanvasAnimator : MonoBehaviour, CanvasSystem.IOnCanvasEnabled, CanvasSystem.IOnCanvasDisabled
{
    public Animator animator;
    public string inAnimName = "in_anim";

    [ContextMenu("Auto Resolve")]
    public void AutoResolve() => animator = GetComponent<Animator>();
    public void OnCanvasDisable() => animator.StopPlayback();
    public void OnCanvasEnable() => animator.Play(inAnimName);
}
