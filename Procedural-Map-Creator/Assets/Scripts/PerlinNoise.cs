using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    [SerializeField] static List<float> NoiseList = new List<float>();

    public static List<float> GenerateNoise(float amp, float freq, float jump, int range)
    {
        float xoff = 0.1f;
        float yoff = xoff + 10.0f;
        for (int i = 0; i < range; i++)
        {

            //Debug.Log(Mathf.PerlinNoise(xoff, yoff) + " " + i + " xoff " + xoff + " yoff " + yoff);
            NoiseList.Add(Mathf.PerlinNoise(xoff * freq, (float)0.1 * freq) * amp);
            xoff += jump;
            yoff += jump;
        }

        return NoiseList;
    }
}
