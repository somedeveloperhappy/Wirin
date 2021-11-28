using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class S2SS_main : MonoBehaviour
{
    public static void Generate(Texture[] textures, int spritesInOneRow, int margin, string path) {
        int columns = spritesInOneRow;
        int rows = Mathf.CeilToInt((float) textures.Length / spritesInOneRow);

        Debug.Log($"generating with row: {rows} columns: {columns}");

        var graphicsFormat = textures[0].graphicsFormat;
        if (!SystemInfo.IsFormatSupported(graphicsFormat, UnityEngine.Experimental.Rendering.FormatUsage.SetPixels)) {
            Debug.LogError("Seems like graphics format : \"" + graphicsFormat.ToString() +
                           "\" is not supported. switching to R16G16B16A16_SFloat.");
            graphicsFormat = UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat;
        }

        Texture2D result = new Texture2D(
            (textures[0].width + margin) * columns - margin,
            (textures[0].height + margin) * rows - margin,
            graphicsFormat,
            UnityEngine.Experimental.Rendering.TextureCreationFlags.None);

        result.anisoLevel = textures[0].anisoLevel;

        for (int row = 0; row < rows; row++) {
            for (int column = 0; column < columns; column++) {
                if (row * columns + column >= textures.Length) break;
                Debug.Log($"at {row}:{column}");

                Texture2D currentTex = (Texture2D) textures[row * columns + column];

                Vector2 startingPosInResultTex = new Vector2(
                    (currentTex.width + margin) * column,
                    (currentTex.height + margin) * row
                );

                // Color color = new Color (UnityEngine.Random.Range (0, 1f), UnityEngine.Random.Range (0, 1f), UnityEngine.Random.Range (0, 1f));

                result.SetPixels(
                    (int) startingPosInResultTex.x,
                    (int) startingPosInResultTex.y,
                    currentTex.width,
                    currentTex.height,
                    currentTex.GetPixels());

                // for (int x = 0; x < currentTex.width; x++) {
                //     for (int y = 0; y < currentTex.height; y++) {

                //         result.SetPixel (
                //             (int) startingPosInResultTex.x + x,
                //             (int) startingPosInResultTex.y + y,
                //             currentTex.GetPixel (x, y)
                //         // color
                //         );

                //     }
                // }
            }
        }

        result.Apply();

        var bytes = result.EncodeToPNG();

        if ((new System.IO.FileInfo(path)).Exists) {
            string newpath_for_old = path;
            while ((new System.IO.FileInfo(path)).Exists) {
                newpath_for_old += ".old";
            }

            Debug.Log($"file {path} already exists. changing the old file's name to {path}+.old");
            System.IO.File.Move(path, newpath_for_old);
        }

        System.IO.File.WriteAllBytes(path, bytes);
        Debug.Log($"Image saved to {path}");
    }

    public static void SlideSprite(string path, int cols, int rows, Texture refTex, float margin) {
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
        ti.isReadable = true;

        List<SpriteMetaData> smd = new List<SpriteMetaData>();

        int count = 0;

        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < cols; col++) {
                SpriteMetaData meta = new SpriteMetaData();
                meta.name = count++.ToString();
                meta.rect = new Rect(
                    (refTex.width + margin) * col,
                    (refTex.height + margin) * row,
                    refTex.width,
                    refTex.height
                );
                meta.alignment = 0;
                meta.pivot = Vector2.zero;
                meta.border = Vector4.zero;
                smd.Add(meta);
            }
        }

        ti.spriteImportMode = SpriteImportMode.Multiple;
        ti.spritesheet = smd.ToArray();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        Debug.Log($"Spritesheet sliced!");
    }
}