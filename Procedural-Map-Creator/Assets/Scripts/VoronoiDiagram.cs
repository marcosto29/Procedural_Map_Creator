using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiDiagram : MonoBehaviour//Class to try and implement a voronoi diagram like algorithm from 0
{

    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] List<Vector3> vertices;
    // Start is called before the first frame update
    void Start()
    {
        Initial();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Initial()//initial method that calculate the mediatrix between N points given
    {
        List<List<Vector3>> midPoints = new List<List<Vector3>>();
        for (int i = 0; i < points; i++)//Creation of balls
        {
            vertices.Add(new Vector3(Random.Range(0, size.x), 0, Random.Range(0, size.y)));
        }

        for (int i = 0; i < points; i++)
        {
            for(int j = i + 1; j < points; j++)
            {
                midPoints.Add(Math.Mediatrix(vertices[i], vertices[j], size));
                Debug.DrawLine(vertices[i], vertices[j], Color.red, 9999999999.9f);//Line
            }
        }

        for(int i = 0; i < midPoints.Count; i++)
        {
            Debug.DrawLine(midPoints[i][0], midPoints[i][1], Color.blue, 9999999999.9f);//Line
        }
    }
}
