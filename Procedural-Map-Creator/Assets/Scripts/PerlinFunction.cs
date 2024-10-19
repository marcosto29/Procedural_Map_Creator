using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinFunction
{
    static float timerCount;
    public static void Draw(Vector2 Limits, LineRenderer Line, int points, float jump, float speed, float amplitude, float frequency)//1D
    {
        timerCount += Time.deltaTime;
        float startingPoint = Limits.x;
        float finishingPoint = Limits.y;
        float xoff = 0;
        float yoff = 10;

        Line.positionCount = points;
        for (int i = 0; i < points; i++)
        {
            float percentage = (float)i / (points - 1);
            float x = Mathf.Lerp(startingPoint, finishingPoint, percentage);
            float y = Math.PerlinNoise(xoff, yoff, timerCount, speed, frequency, amplitude);//here tons of perlin noise can be calculated and added with different amplitudes and frequencies
            float y2 = Math.PerlinNoise(xoff, yoff, timerCount, speed, frequency * 2, amplitude / 2);
            float y3 = Mathf.Sin(x);
            float y4 = y3 + y2 + y;
            Line.SetPosition(i, new Vector3(x, y4, Line.gameObject.transform.position.z));
            xoff += jump;
            yoff += jump;
        }
    }

    public static void DDraw(Mesh plane, Vector2 def, Vector3[] vertices, float speed, float amplitude, float frequency, float jump)//2 and 3D
    {
        vertices = plane.vertices;
        float[] values = new float[vertices.Length];

        timerCount += Time.deltaTime;
        float xoff = 0;
        float yoff = 10;
        int count = 0;

        for (int i = 0; i <= def.y; i++)
        {
            for (int j = 0; j <= def.x; j++)
            {
                float y = Math.PerlinNoise(xoff, yoff, timerCount, speed, frequency, amplitude*2);

                values[count] = y;
                xoff += jump;
                count++;
            }
            xoff = 0;
            yoff += jump;
        }

        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = values[i];
        }

        plane.vertices = vertices;
    }
}
