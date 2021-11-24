[System.Serializable]
public struct MinMax
{
    public float min, max;
    public float Evaluate(float t) => UnityEngine.Mathf.Lerp(min, max, t);
}

[System.Serializable]
public struct MinMax<T>
{
    public T min, max;
}
