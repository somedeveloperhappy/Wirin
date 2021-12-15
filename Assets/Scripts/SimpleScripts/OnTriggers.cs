using UnityEngine;

public class OnTriggers : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent<Collider2D> onTrigegrEnter;
    public UnityEngine.Events.UnityEvent<Collider2D> onTrigegrExit;
    public UnityEngine.Events.UnityEvent<Collider2D> onTrigegrStay;


    private void OnTriggerEnter2D(Collider2D other) => onTrigegrEnter?.Invoke(other);
    private void OnTriggerExit2D(Collider2D other) => onTrigegrExit?.Invoke(other);
    private void OnTriggerStay2D(Collider2D other) => onTrigegrStay?.Invoke(other);
}
