using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public static float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    public static float Lerp(float first, float second, float percentage)//calculate the number between two values given an X percentage
    {
        float difference = second - first;//calculate the distance between the two values
        return first + (percentage * difference);//add the percentage required to the distance calculated and add it to the first value
    }

    public static float PerlinNoise(float x, float y, float timerCount, float speed, float frequency, float amplitude)
    {
        return Mathf.PerlinNoise((x + timerCount * speed) * frequency, (y + timerCount * speed) * frequency) * amplitude;//The variable timerCount makes it so that the graph changes over time and the variable speed how fast does it
    }
}
