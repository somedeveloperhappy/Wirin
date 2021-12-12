using UnityEngine;

namespace UI
{
	public class Scrollable : MonoBehaviour
	{
		public float viewDistance = 250;
		
		public bool isOutsideView()
		{
			var pos = this.transform.position;
			return pos.x + viewDistance < 0 || pos.x - viewDistance > Screen.width || pos.y + viewDistance < 0 || pos.y - viewDistance > Screen.height;
		}
	}
}
