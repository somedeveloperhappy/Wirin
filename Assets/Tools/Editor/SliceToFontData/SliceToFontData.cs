using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

class SliceToFontData : EditorWindow
{
    [MenuItem("Wirin/Slice to font data")]
    static void ShowWindow()
    {
        var window = GetWindow<SliceToFontData>();
        window.titleContent = new GUIContent("Slice To Font Data");
        window.Show();
    }

    public UnityEngine.Font font;
    public string texturePath;
    public float advanceAdditiontoWidth = 10;
    public int startAscii = 48;

    private void OnGUI()
    {
        var serializedObject = new SerializedObject(this);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(texturePath)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(advanceAdditiontoWidth)));
        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(startAscii)));

        if (GUILayout.Button("Make"))
        {
            var ti = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            Debug.Log($"texture has {ti.spritesheet.Length}slices.");

            // set the size
            font.characterInfo = new CharacterInfo[ti.spritesheet.Length];

            float width, height;
            GetImageSize(texturePath, out width, out height);

            if (width == 0 || height == 0)
                throw new System.Exception("Size could not be get!");
            Debug.Log($"size : {width} , {height}");

            List<CharacterInfo> infos = new List<CharacterInfo>();

            // set data for each one
            for (int i = 0; i < ti.spritesheet.Length; i++)
            {
                var spr = ti.spritesheet[i];
                var info = new CharacterInfo()
                {
                    vert = new Rect(spr.rect.x, spr.rect.y, spr.rect.width, -spr.rect.height),
                    uv = new Rect(
                        spr.rect.x / width, spr.rect.y / height,
                        spr.rect.width / width, spr.rect.height / height),
                    advance = (int)(spr.rect.width + advanceAdditiontoWidth),
                    index = int.Parse(spr.name) + startAscii - 48
                };
                infos.Add(info);
            }
            font.characterInfo = infos.ToArray();

            EditorUtility.SetDirty(font);
            Repaint();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public static bool GetImageSize(string assetPath, out float width, out float height)
    {
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer != null)
        {
            if (importer != null)
            {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);

                width = (float)(int)args[0];
                height = (float)(int)args[1];

                return true;
            }
        }

        height = width = 0;
        return false;
    }
}