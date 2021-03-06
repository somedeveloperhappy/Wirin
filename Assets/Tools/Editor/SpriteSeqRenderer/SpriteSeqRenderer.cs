using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SpriteSeqRenderer : MonoBehaviour
{
    public float animPrecision = 0.05f;
    private Camera cam;
    public string path;

    public List<Texture2D> records = new List<Texture2D>();

    public Animator[] renderingAnimators;

    public RenderTexture rt;

    private float t;
    public float timeToFinish = 1;

    private void Start()
    {
        path = EditorUtility.SaveFilePanelInProject("Save rendered PNGs", "sprite", "", "");

        foreach (var anim in renderingAnimators) anim.enabled = false;
    }

    private void Update()
    {
        foreach (var anim in renderingAnimators) anim.Update(animPrecision);

        t += animPrecision;
        if (t >= timeToFinish)
        {
            enabled = false;
            SavePNGsToFile();
            Debug.Break();
        }
    }

    private void SavePNGsToFile()
    {
        for (var i = 0; i < records.Count; i++)
        {
            var bytes = records[i].EncodeToPNG();
            File.WriteAllBytes(path + "_" + i + ".png", bytes);
        }

        Debug.Log("Saved!");
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }


    private void OnPostRender()
    {
        RenderTexture.active = rt;
        var tex = new Texture2D(rt.width, rt.height, rt.graphicsFormat,
            TextureCreationFlags.None);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        records.Add(tex);
    }


    // private void OnRenderImage(RenderTexture src, RenderTexture dest) {
    //     RenderTexture.active = src;
    //     Texture2D tex = new Texture2D (src.width, src.height, src.graphicsFormat, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
    //     tex.ReadPixels (new Rect (0, 0, tex.width, tex.height), 0, 0);
    //     records.Add (tex);
    //     dest = src;
    // }
}