using UnityEngine;
using UnityEditor;
using System;

namespace LevelManaging
{
    [CustomEditor(typeof(LineSegments))]
    public class LineSegmentsEditor : Editor
    {
        [Obsolete]
        void OnEnable() {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        [Obsolete]
        void OnDisable() {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        private void OnSceneGUI(SceneView sceneView) 
        {
            serializedObject.Update();
            
            var tar = target as LineSegments;
            
            Handles.color = Color.green;

            var points_prop = serializedObject.FindProperty("points");
            
            for (ushort i = 0; i < points_prop.arraySize; i++) {
                EditorGUI.BeginChangeCheck();
                points_prop.GetArrayElementAtIndex(i).vector2Value = Handles.PositionHandle(points_prop.GetArrayElementAtIndex(i).vector2Value, Quaternion.identity);
                Handles.DrawLine(points_prop.GetArrayElementAtIndex(i).vector2Value, points_prop.GetArrayElementAtIndex((i+1)%points_prop.arraySize).vector2Value);
                if(EditorGUI.EndChangeCheck()) {
                    tar.CalculateMaxX();
                    serializedObject.FindProperty("maximumX").floatValue = tar.maximumX;
                }
            }
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
