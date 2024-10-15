using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PerlinFunction : MonoBehaviour
{
    [SerializeField] private LineRenderer Line;
    [SerializeField] private int points;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private Vector2 Limits;
    [SerializeField] Mesh plane;
    [SerializeField] Vector3[] vertex;

    private float timerCount;
    void Start()
    {
        //Line = GetComponent<LineRenderer>();
        //plane = GetComponent<MeshFilter>().mesh;//get the Mesh of a plane to manipulate its vertex
        //DDraw();
    }

    void Draw()//1D
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
            Line.SetPosition(i, new Vector3(x, y4, 0));
            xoff += jump;
            yoff += jump;
        }
    }

    public void DDraw(MeshFilter plane)//2 and 3D
    {
        vertex = plane.mesh.vertices;
        float[] values = new float[vertex.Length];

        timerCount += Time.deltaTime;
        float xoff = 0;
        float yoff = 10;
        int count = 0;

        for (int i = 0; i < Mathf.Sqrt(vertex.Length); i++)//this works on square vertex, not rectangles, gotta fix it
        {
            for (int j = 0; j < Mathf.Sqrt(vertex.Length); j++)
            {
                float y = Math.PerlinNoise(xoff, yoff, timerCount, speed, frequency, amplitude*2);

                values[count] = y;
                xoff += jump;
                count++;
            }
            xoff = 0;
            yoff += jump;
        }

        count = 0;

        for (int i = 0; i < Mathf.Sqrt(vertex.Length); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(vertex.Length); j++)
            {
                count++;
            }
        }




        for(int i = 0; i < vertex.Length; i++)
        {
            vertex[i].y = values[i];
        }

        plane.mesh.vertices = vertex;
    }

    void Update()
    {
        //DDraw();
        //Draw();
    }
}
