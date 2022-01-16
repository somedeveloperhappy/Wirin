using System.Collections.Generic;
using UnityEngine;


static public class FunctionGrapher
{
    public delegate float Equation(float x);
    static Dictionary<Equation, Texture> caches = new Dictionary<Equation, Texture>();

    static Color blankCol = Color.black;
    static Color whiteCol = Color.white;

    public static Texture GetEquationTexture(Equation equation, Vector2 start, Vector2 end, int width = 100, int height = 90)
    {

        if (caches.ContainsKey(equation))
        {
            return caches[equation];
        }

        Texture2D tex = new Texture2D(width, height);

        for (int i = 0; i < width; i++)
        {
            float x = start.x + (end.x - start.x) * (i / (float)width);
            int y = (int)Mathf.Lerp(0, height, Mathf.InverseLerp(start.y, end.y, equation(x)));

            for (int j = 0; j < height; j++)
            {
                Color col;
                if (j == y) col = whiteCol;
                else if (Mathf.Abs(j - y) <= 6) col = Color.Lerp(whiteCol, blankCol, Mathf.Abs(j - y) / 6f);
                else col = blankCol;
                tex.SetPixel(i, j, col);
            }
        }
        tex.Apply();


        caches.Add(equation, tex);
        return Texture2D.blackTexture;
    }
}
