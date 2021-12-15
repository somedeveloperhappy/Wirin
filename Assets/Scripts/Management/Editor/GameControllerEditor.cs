using Management;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(15);

        if (GUILayout.Button("Resolve Automatically", GUILayout.Height(20)))
        {
            var tar = target as GameController;

            var canvases = FindObjectsOfType<CanvasSystem.CanvasBase>();

            void ResolveCanvas(string name)
            {
                (CanvasSystem.CanvasBase canvas, int nameEqualPoint) bestMatch = (null, int.MaxValue);

                foreach (var canvas in canvases)
                {
                    if (bestMatch.canvas == null)
                    {
                        bestMatch.canvas = canvas;
                        bestMatch.nameEqualPoint = SimpleScripts.HandyFuncs.str_eq_point(name, canvas.name);
                        continue;
                    }

                    int nameEq = SimpleScripts.HandyFuncs.str_eq_point(name, canvas.name);

                    if (nameEq > bestMatch.nameEqualPoint)
                    {
                        bestMatch.canvas = canvas;
                        bestMatch.nameEqualPoint = nameEq;
                        continue;
                    }

                }

                if (bestMatch.canvas == null) return;
                // by now we should have the answer
                serializedObject.FindProperty(name).objectReferenceValue = bestMatch.canvas;
            }

            // resolve for each canvas base
            ResolveCanvas(nameof(tar.ingameCanvas));
            ResolveCanvas(nameof(tar.winMenuCanvas));
            ResolveCanvas(nameof(tar.loseMenuCanvas));
            ResolveCanvas(nameof(tar.mainMenuCanvas));

            serializedObject.ApplyModifiedProperties();
        }


    }


}
