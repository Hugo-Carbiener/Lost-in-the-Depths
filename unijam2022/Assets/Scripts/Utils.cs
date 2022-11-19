using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static List<Vector2Int> directNeihbors = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    public static List<Vector2Int> neihbors = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down };
    public static List<Vector2Int> radius2Sphere = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down, 2 * Vector2Int.up, 2 * Vector2Int.down, 2 * Vector2Int.left, 2 * Vector2Int.right };
    public static List<Vector2Int> squaredRadius2Sphere = new List<Vector2Int>() { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.one, -Vector2Int.one, Vector2Int.left + Vector2Int.up, Vector2Int.right + Vector2Int.down, 2 * Vector2Int.up, 2 * Vector2Int.down, 2 * Vector2Int.left, 2 * Vector2Int.right, 2 * Vector2Int.up + Vector2Int.left,2 * Vector2Int.up + Vector2Int.right, 2 * Vector2Int.down + Vector2Int.left, 2 * Vector2Int.down + Vector2Int.right, 2 * Vector2Int.left + Vector2Int.down, 2 * Vector2Int.left + Vector2Int.up, 2 * Vector2Int.right + Vector2Int.up, 2 * Vector2Int.right + Vector2Int.down };
    public static float RandomGaussian(float minValue = 0.0f, float maxValue = 1.0f)
    {
        float u, v, S;

        do
        {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);

        // Normal Distribution centered between the min and max value
        // and clamped following the "three-sigma rule"
        float mean = (minValue + maxValue) / 2.0f;
        float sigma = (maxValue - mean) / 3.0f;
        return Mathf.Clamp(std * sigma + mean, minValue, maxValue);
    }
}