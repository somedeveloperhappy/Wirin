using UnityEngine;
using UnityEngine.Events;

public class OnCollision : MonoBehaviour
{
	public UnityEvent<Collision2D> onCollisionEnter;
	public UnityEvent<Collision2D> onCollisionExit;
	public UnityEvent<Collision2D> onCollisionStay;

	private void OnCollisionEnter2D(Collision2D other)
	{
		onCollisionEnter?.Invoke(other);
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		onCollisionExit?.Invoke(other);
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		onCollisionStay?.Invoke(other);
	}
}