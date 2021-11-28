using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent<Collision2D> onCollisionEnter;
    public UnityEngine.Events.UnityEvent<Collision2D> onCollisionExit;
    public UnityEngine.Events.UnityEvent<Collision2D> onCollisionStay;

    private void OnCollisionEnter2D(Collision2D other) => onCollisionEnter?.Invoke(other);
    private void OnCollisionExit2D(Collision2D other) => onCollisionExit?.Invoke(other);
    private void OnCollisionStay2D(Collision2D other) => onCollisionStay?.Invoke(other);
}