using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelaunayTriangulation : MonoBehaviour
{

    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] List<Vector3> vertices;
    // Start is called before the first frame update
    void Start()
    {
        //before doing the bisectors calculations, i need to do a delaunay triangulation to eliminate unnecessary triangles 
        InitialVertices();
        SortPoints();
        CheckColinear(vertices);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialVertices()//initial method that calculate the mediatrix between N points given
    {
        for (int i = 0; i < points; i++)//Creation of balls
        {
            vertices.Add(new Vector3(UnityEngine.Random.Range(0, size.x), 0, UnityEngine.Random.Range(0, size.y)));
        }
    }

    void SortPoints()//para mañana, este metodo solo va a servir para desplazar el puntero a donde le corresponde a partir ahí ya enviara el array correspondiente de puntos para que se hagan las operaciones convenientes
    {
        //para saber si un punto está en el círculo recuerda que basta con comprbar que la distancia del centro al punto sea menor que el radio, y buena suerte limpiando todo esto
        List<int> hull1 = new List<int>();
        List<int> hull2 = new List<int>();
        vertices.Sort((a, b) => a.x.CompareTo(b.x));//vertices sorted by X value least to biggest with lambda expression
        int pointer = vertices.Count / 2;
        while(pointer > 2) pointer /= 2; //get the pointer to where the edge needs to be created
        List<Vector3> chosenPoints = new List<Vector3>();
        for(int i = 0; i <= pointer; i++) chosenPoints.Add(vertices[i]); //enter the remaining vertices on a List to work with them
        hull1 = JoinPoints(chosenPoints);
        chosenPoints.Clear();
        for (int i = 1; i <= 3 && pointer + i < vertices.Count; i++) chosenPoints.Add(vertices[pointer + i]); //enter the other section of the vertices
        hull2 = JoinPoints(chosenPoints);
    }

    List<int> JoinPoints(List<Vector3> vertices)
    {
        List<int> hull = new List<int>();
        //check wether the vertices are colinear to instead of drawing a triangle just join them
        bool line = CheckColinear(vertices);
        if (line)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                hull.Add(i);
                if(i > 0)
                {
                    Debug.DrawLine(vertices[i-1], vertices[i], Color.blue, 99999999.9f);
                }
            }
        }
        else//to join the points, is neccessary to established a left to right join algorithm to make a counter-clockwise triangle 
        {
            //check if they are counter-clockwise
            Vector3 aux1 = vertices[1] - vertices[0];
            Vector3 aux2 = vertices[2] - vertices[0];
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            print(crossProductZ);
            if(crossProductZ <= 0)//if they are clockwise swap them
            {
                Vector3 temp = vertices[1];
                vertices[1] = vertices[2];
                vertices[2] = temp;
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                hull.Add(i);
                if (i > 0)
                {
                    Debug.DrawLine(vertices[i - 1], vertices[i], Color.blue, 99999999.9f);
                }
            }
        }
        return hull;

    }

    bool CheckColinear(List<Vector3> vertices)
    {
        //Ax * (By - Cy) + Bx * (Cy - Ay) + Cx * (Ay - By) / 2 check if the area of the triangle is 0, doesnt need to divide by 2
        if (vertices.Count >= 3) return (vertices[0].x * (vertices[1].z - vertices[2].z) + vertices[1].x * (vertices[2].z - vertices[0].z) + vertices[2].x * (vertices[0].z - vertices[1].z)) == 0;
        //when handling only 2 points catch the exception and send a true since a line will be created
        return true;
    }
}
