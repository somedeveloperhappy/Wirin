using UnityEngine;

namespace CanvasSystem
{
	public class CanvasSystemOperator : MonoBehaviour
	{
		public IOnCanvasDisabled[] canvasDisableds;
		public IOnCanvasEnabled[] canvasEnableds;


		// this canvas
		private Canvas m_canvas;


		[ContextMenu("fill automatically")]
		public void FillAutomatically()
		{
			canvasEnableds = GetComponentsInChildren<IOnCanvasEnabled>();
			canvasDisableds = GetComponentsInChildren<IOnCanvasDisabled>();
		}

		private void Awake()
		{
			FillAutomatically();
			m_canvas = GetComponent<Canvas>();
			enabled = m_canvas.enabled;
		}

		private void OnEnable()
		{
			m_canvas.enabled = true;
			foreach (var obj in canvasEnableds) obj.OnCanvasEnable();
		}

		private void OnDisable()
		{
			m_canvas.enabled = false;
			foreach (var obj in canvasDisableds) obj.OnCanvasDisabled();
		}
	}
}