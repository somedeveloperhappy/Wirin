using System.Collections.Generic;
using UnityEngine;

namespace CanvasSystem
{
    public class CanvasBase : MonoBehaviour
    {
        public IOnCanvasDisabled[] canvasDisableds;
        public IOnCanvasEnabled[] canvasEnableds;

        public Transform[] ignoreTransformTrees;

        // this canvas
        private Canvas m_canvas;


        [ContextMenu("fill automatically")]
        public void FillAutomatically()
        {
            canvasEnableds = GetComponentsInChildrenExceptIgnored<IOnCanvasEnabled>();
            canvasDisableds = GetComponentsInChildrenExceptIgnored<IOnCanvasDisabled>();
        }

        private T[] GetComponentsInChildrenExceptIgnored<T>()
        {
            List<T> ignoreinstances = new List<T>();
            foreach (var ignore in ignoreTransformTrees)
                ignoreinstances.AddRange(ignore.GetComponentsInChildren<T>());

            var allInstances = GetComponentsInChildren<T>();

            List<T> true_instances = new List<T>();

            for (int i = 0; i < allInstances.Length; i++)
            {
                if (!ignoreinstances.Contains(allInstances[i]))
                    true_instances.Add(allInstances[i]);
            }
            return true_instances.ToArray();
        }

        private void Awake()
        {
            FillAutomatically();
            m_canvas = GetComponent<Canvas>();
            enabled = m_canvas.enabled;
        }

        private void OnEnable()
        {
            Debug.Log($"{name} enabled");
            FillAutomatically();
            m_canvas.enabled = true;
            foreach (var obj in canvasEnableds) obj.OnCanvasEnable();
        }

        private void OnDisable()
        {
            FillAutomatically();
            m_canvas.enabled = false;
            foreach (var obj in canvasDisableds) obj.OnCanvasDisable();
        }
    }
}
