using System.Collections.Generic;
using ThemeSystem;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ThemeBased<>))]
public class ThemeBasedEditor : PropertyDrawer
{
    Dictionary<string, float> totalHeight = new Dictionary<string, float>();

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, property, label, true);


        EditorGUI.EndProperty();
        var val_prop = property.FindPropertyRelative("values");

        var len = PlayerPrefs.GetString("themes", string.Empty).Length;
        if (val_prop.arraySize != len)
        {
            val_prop.arraySize = len;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        if (ThemeManager.instance != null)
        {
            for (int i = 0; i < val_prop.arraySize; i++)
            {
                val_prop.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue = ThemeManager.instance.themes[i].name;
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}


[CustomPropertyDrawer(typeof(ThemeBased<>.Values))]
public class ThemeBasedValueEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUI.PropertyField(position, property.FindPropertyRelative("val"), label, true);

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("val"), true);
    }
}
