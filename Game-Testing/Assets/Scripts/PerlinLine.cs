using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PerlinLine : MonoBehaviour
{
    [SerializeField] private LineRenderer Line;
    [SerializeField] private int points;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private Vector2 Limits;

    private float timerCount;
    void Start()
    {
        Line = GetComponent<LineRenderer>();
    }

    void Draw()
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
            Line.SetPosition(i, new Vector3(x, y, 0));
            xoff += jump;
            yoff += jump;
        }
    }

    void Update()
    {
        Draw();
    }
}
