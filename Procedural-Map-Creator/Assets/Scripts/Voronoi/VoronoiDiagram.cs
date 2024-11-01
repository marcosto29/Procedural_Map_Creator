using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour//Class to try and implement a voronoi diagram like algorithm from 0
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Bisectors(int points, List<Vector3> vertices, Vector2 size)
    {
        List<List<Vector3>> midPoints = new List<List<Vector3>>();
        for (int i = 0; i < points; i++)
        {
            for (int j = i + 1; j < points; j++)
            {
                midPoints.Add(Math.Bisector(vertices[i], vertices[j], size));//calculate the boundary points that collides with the bisector to draw it
                Debug.DrawLine(vertices[i], vertices[j], Color.red, 9999999999.9f);//Line, also i need to find a betterr way to draw lines, i think the line render could be cool once the delaunay triangulation is done
            }
        }

        for (int i = 0; i < midPoints.Count; i++) Debug.DrawLine(midPoints[i][0], midPoints[i][1], Color.blue, 9999999999.9f);//bisector line
    }
}
