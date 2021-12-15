using UnityEditor;
using UnityEngine;

namespace Management.Editor
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            if (target == null) return;

            (target as LevelManager).LoadLevelNumberFromPrefs();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Clear playerPrefs", GUILayout.Height(25))) PlayerPrefs.DeleteAll();
        }
    }
}