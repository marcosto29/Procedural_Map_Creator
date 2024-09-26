using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PerlinLine : MonoBehaviour
{
    [SerializeField] private LineRenderer myLineRenderer;
    [SerializeField] private int points;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private new GameObject camera;
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
    }

    void Draw()
    {
        float xStart = -10;
        float xFinish = 10;

        myLineRenderer.positionCount = points;
        for (int currentPoint = 0; currentPoint < points; currentPoint++)
        {
            float progress = (float)currentPoint / (points - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = (Mathf.PerlinNoise(x * frequency, (float)0.1) * frequency) * amplitude;
            myLineRenderer.SetPosition(currentPoint, new Vector3(x, y, 0));
        }
    }

    void Update()
    {
        Draw();
    }
}
