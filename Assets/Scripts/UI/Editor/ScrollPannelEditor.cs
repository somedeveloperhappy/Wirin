using UI;
using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( ScrollPannel ) )]
public class ScrollPannelEditor : Editor
{
    private const int line_len = 600;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        var tar = target as ScrollPannel;

        if (GUILayout.Button( "Get Auto References" ))
        {
            Undo.RecordObject( tar, "scroll pannel" );
            EditorUtility.SetDirty( serializedObject.targetObject );
            tar.GetAutoReferences();
        }

        if (GUILayout.Button( "Reposition Subs" ))
        {
            Transform[] scs = new Transform[tar.scrollables.Length];
            for (int i = 0; i < scs.Length; i++) scs[i] = tar.scrollables[i].transform;

            Undo.RecordObjects( scs, "scroll pannel" );
            EditorUtility.SetDirty( serializedObject.targetObject );
            tar.RepositionSubs();
        }

        if (GUILayout.Button( "Disable Outside View" ))
        {
            // making undo
            GameObject[] scs = new GameObject[tar.scrollables.Length];
            for (int i = 0; i < scs.Length; i++) scs[i] = tar.scrollables[i].gameObject;

            Undo.RecordObjects( scs, "scroll pannel" );

            EditorUtility.SetDirty( serializedObject.targetObject );
            tar.DisableOutsideView();
        }

        if (GUILayout.Button( "Auto Set Boundries" ))
        {
            Undo.RecordObject( tar, "scroll pannel" );
            EditorUtility.SetDirty( serializedObject.targetObject );
            tar.AutoSetXBoundries();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        Handles.color = Color.red;

        var tar = target as ScrollPannel;

        Handles.DrawSolidDisc( tar.transform.position + (Vector3)tar.transformSettings.ofsset, Vector3.back, 0.15f );

        Handles.color = Color.blue;

        Vector3 left = tar.targetTransform.position + Vector3.right * tar.scrolXBoundries.min;
        Handles.DrawLine( left + Vector3.up * line_len, left + Vector3.down * line_len, 5 );

        Vector3 right = tar.targetTransform.position + Vector3.right * tar.scrolXBoundries.max;
        Handles.DrawLine( right + Vector3.up * line_len, right + Vector3.down * line_len, 5 );
    }
}
