using UnityEngine;
using UnityEditor;

namespace FSystem
{
    public partial class FObject : MonoBehaviour
    {

        [CustomEditor(typeof(FObject))]
        public class FObjectEditor : Editor
        {
            float total_height;

            public override void OnInspectorGUI()
            {
                var position = EditorGUILayout.BeginVertical();
                var first_y = position.y;

                position.height = EditorGUIUtility.singleLineHeight;

                var tar = target as FObject;

                var components = serializedObject.FindProperty("components");






                total_height = position.y + position.height - first_y;
                EditorGUILayout.Space(total_height);

                EditorGUILayout.EndVertical();

            }
            
            

        }
    }
}