using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public Bounds bounds => spriteRenderer.bounds;
    
    public SpriteRenderer spriteRenderer;
}
