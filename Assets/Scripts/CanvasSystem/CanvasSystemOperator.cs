using UnityEngine;
using System.Collections.Generic;

namespace CanvasSystem
{
    public class CanvasSystemOperator : MonoBehaviour
    {
        public IOnCanvasEnabled[] canvasEnableds;
        public IOnCanvasDisabled[] canvasDisableds;


        [ContextMenu("fill automatically")]
        public void FillAutomatically() {
            canvasEnableds = GetComponentsInChildren<IOnCanvasEnabled>();
            canvasDisableds = GetComponentsInChildren<IOnCanvasDisabled>();
        }


        // this canvas
        Canvas m_canvas;

        private void Awake() {
            FillAutomatically();
            m_canvas = GetComponent<Canvas>();
            this.enabled = m_canvas.enabled;
        }

        private void OnEnable() {
            m_canvas.enabled = true;
            foreach (var obj in canvasEnableds) obj.OnCanvasEnable();
        }

        private void OnDisable() {
            m_canvas.enabled = false;
            foreach (var obj in canvasDisableds) obj.OnCanvasDisabled();
        }
    }
}