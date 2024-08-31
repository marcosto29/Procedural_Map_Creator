using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    [SerializeField] static List<float> NoiseList = new List<float>();

    public static List<float> GenerateNoise()
    {
        float xoff = 0.1f;
        float yoff = xoff + 10.0f;
        float jump = 10.0f;
        float amplitude = 95.5f;
        float frequency = 5.0f;
        for(int i = 0; i < 1000; i++)
        {
            //Debug.Log(Mathf.PerlinNoise(xoff, yoff) + " " + i + " xoff " + xoff + " yoff " + yoff);
            NoiseList.Add(Mathf.PerlinNoise(amplitude * xoff, amplitude * yoff) * frequency);
            xoff+=jump;
            yoff+=jump;
        }

        return NoiseList;
    }

}
