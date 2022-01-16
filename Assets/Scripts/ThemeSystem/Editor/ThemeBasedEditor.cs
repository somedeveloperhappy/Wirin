using ThemeSystem;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ThemeBased<>))]
public class ThemeBasedEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndProperty();
        var val_prop = property.FindPropertyRelative("values");

        if (ThemeManager.themes != null)
        {
            var len = ThemeManager.themes.Length;
            if (val_prop.arraySize != len)
            {
                val_prop.arraySize = len;
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label);
    }
}


//[CustomPropertyDrawer(typeof(ThemeBased<>.Values))]
//public class ThemeBasedValueEditor : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        position.height = EditorGUIUtility.singleLineHeight;
//        label = EditorGUI.BeginProperty(position, label, property);

//        EditorGUI.PropertyField(position, property.FindPropertyRelative("val"), label, true);

//        EditorGUI.EndProperty();
//    }

//    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//    {
//        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("val"), true);
//    }
//}
