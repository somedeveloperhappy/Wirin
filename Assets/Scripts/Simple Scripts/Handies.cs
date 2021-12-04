using System;
using UnityEngine;

[Serializable]
public struct MinMax
{
	public float min, max;

	public float Evaluate(float t)
	{
		return Mathf.Lerp(min, max, t);
	}
}

[Serializable]
public struct MinMax<T>
{
	public T min, max;
}