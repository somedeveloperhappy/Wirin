using Microsoft.Win32;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class PrefsEditor : EditorWindow
{

    private const string DELETE_WORD = "del";
    public Dictionary<string, string> modifieds = new Dictionary<string, string>();

    private (string key, string value) newValue = (string.Empty, string.Empty);

    private string path;
    public Dictionary<string, string> prefs = new Dictionary<string, string>();

    private float width;

    [MenuItem("Wirin/Prefs Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<PrefsEditor>();
        window.titleContent = new GUIContent("Prefs");
        window.Show();
    }

    private void OnEnable()
    {
        path = string.Format(@"SOFTWARE\Unity\UnityEditor\{0}\{1}", Application.companyName, Application.productName);
        GetPrefs();
    }

    private void GetPrefs()
    {
        prefs.Clear();

        var basekey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey(path);

        var keys = basekey.GetValueNames();

        foreach (var key in keys)
        {
            var last_ = key.LastIndexOf("_");
            var keypref = key.Substring(0, last_);

            var val = basekey.GetValue(key);

            if (val is byte[])
                prefs[keypref] = Encoding.UTF8.GetString((byte[])val);
            else
                prefs[keypref] = val.ToString();
        }

        width = EditorGUIUtility.fieldWidth;
    }

    private void OnGUI()
    {
        var guicol = GUI.color;

        EditorGUILayout.LabelField($"path : {path}");

        if (GUILayout.Button("Refresh")) GetPrefs();


        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name");
        EditorGUILayout.LabelField("Current value");
        // EditorGUILayout.LabelField (">>", GUILayout.Width(20));
        EditorGUILayout.LabelField("    New value");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);


        foreach (var pref in prefs)
        {
            if (modifieds.ContainsKey(pref.Key) && modifieds[pref.Key] != string.Empty)
            {
                if (modifieds[pref.Key] == DELETE_WORD) GUI.color = Color.red;
                else GUI.color = Color.green;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(pref.Key);
            EditorGUILayout.LabelField(pref.Value);
            EditorGUILayout.LabelField(">>", GUILayout.Width(20));
            EditorGUI.BeginChangeCheck();

            modifieds[pref.Key] =
                EditorGUILayout.TextField(modifieds.ContainsKey(pref.Key) ? modifieds[pref.Key] : string.Empty);
            EditorGUILayout.EndHorizontal();

            GUI.color = guicol;
        }

        if (GUILayout.Button("Change", GUILayout.Height(30)))
        {
            foreach (var mod in modifieds)
            {
                Debug.Log( $"Setting key {mod.Key} val {mod.Value}" );
                if (mod.Value == string.Empty) continue;

                if (mod.Value == DELETE_WORD)
                    PlayerPrefs.DeleteKey(mod.Key);
                else
                    PlayerPrefs.SetString(mod.Key, mod.Value);
                Debug.Log( $"key {mod.Key} is now {PlayerPrefs.GetString( mod.Key )}" );
            }

            GetPrefs();
            modifieds.Clear();
        }

        EditorGUILayout.Space(15);
        EditorGUILayout.BeginHorizontal();
        newValue.key = EditorGUILayout.TextField("key", newValue.key);
        newValue.value = EditorGUILayout.TextField("Value", newValue.value);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create"))
        {
            PlayerPrefs.SetString(newValue.key, newValue.value);
            newValue = (string.Empty, string.Empty);
            GetPrefs();
        }
    }
}
