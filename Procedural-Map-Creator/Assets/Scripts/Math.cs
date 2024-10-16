using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public static float Lerp(float first, float second, float percentage)//calculate the number between two values given an X percentage
    {
        float totalDistance = second - first;//calculate the distance between the two values
        return first + (percentage * totalDistance);//multiply the percentage required to the totalDistance distance and add it to the first value to get the wanted point
    }

    public static float PerlinNoise(float x, float y, float timerCount, float speed, float frequency, float amplitude)
    {
        //unlike Random function when calculating a Perlin Value it does it through a number time variable (the value is fixed on that seed) and not between the range of 2 number variables
        return Mathf.PerlinNoise((x + timerCount * speed) * frequency, (y + timerCount * speed) * frequency) * amplitude;//The variable timerCount makes it so that the graph changes over time and the variable speed how fast does it
    }
}
