using UnityEngine;
using UnityEditor;
using UI;

[CustomEditor (typeof (Scrollable)), DrawGizmo(GizmoType.Active), CanEditMultipleObjects]
public class ScrollableEditor : Editor
{
	
	private void OnSceneGUI() 
	{
		Handles.color = Color.green;
		var tar = target as Scrollable;
		
		var left = tar.transform.position + Vector3.left * tar.viewDistance;
		Handles.DrawLine(left + Vector3.down * 200, left + Vector3.up * 200);
		var right = tar.transform.position + Vector3.right * tar.viewDistance;
		Handles.DrawLine(right + Vector3.down * 200, right + Vector3.up * 200);
	}
	private void OnDrawGizmosSelected() 
	{
		Debug.Log($"oooo");
		// Gizmos.DrawCube((target as Scrollable).transform.position, Vector3.one * 100);
	}
}
