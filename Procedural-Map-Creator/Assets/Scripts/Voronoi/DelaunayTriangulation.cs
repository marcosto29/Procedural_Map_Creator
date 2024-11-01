using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] LinkedList<Vector3> vertices;
    // Start is called before the first frame update
    void Start()
    {
        vertices = new LinkedList<Vector3>();
        //before doing the bisectors calculations, i need to do a delaunay triangulation to eliminate unnecessary triangles 
        InitialVertices();
        Divide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitialVertices()//initial method that calculate the mediatrix between N points given
    {
        for (int i = 0; i < points; i++)
        {
            vertices.Add(new Vector3(Random.Range(0, size.x), 0, Random.Range(0, size.y))); //random vertices position
        }
        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest
        vertices.AscendingSort(new ComparerV());//putting the comparer
    }

    void Divide()//para mañana, este metodo solo va a servir para desplazar el puntero a donde le corresponde a partir ahí ya enviara el array correspondiente de puntos para que se hagan las operaciones convenientes
    {


        ////para saber si un punto está en el círculo recuerda que basta con comprbar que la distancia del centro al punto sea menor que el radio, y buena suerte limpiando todo esto
        //List<Vector3> hull1 = new List<Vector3>();
        //List<Vector3> hull2 = new List<Vector3>();
        //vertices.Sort((a, b) => a.x.CompareTo(b.x));//vertices sorted by X value least to biggest with lambda expression
        //int pointer = vertices.Count / 2;
        //while(pointer > 2) pointer /= 2; //get the pointer to where the edge needs to be created


       
        ////to select which hulls and vertices are gonna be needed to work on is neccessary something different
        //List<Vector3> chosenPoints = new List<Vector3>();
        //for(int i = 0; i <= pointer; i++) chosenPoints.Add(vertices[i]); //enter the remaining vertices on a List to work with them
        //hull1 = CreateHulls(chosenPoints);
        //chosenPoints.Clear();
        //for (int i = 1; i <= 3 && pointer + i < vertices.Count; i++) chosenPoints.Add(vertices[pointer + i]); //enter the other section of the vertices
        //hull2 = CreateHulls(chosenPoints);


        //Conquer(hull1, hull2);
    }

    List<Vector3> CreateHulls(List<Vector3> vertices)
    {
        //List<Vector3> hull = new List<Vector3>();
        ////check wether the vertices are colinear to instead of drawing a triangle just join them
        //bool line = CheckColinear(vertices);
        //if (!line)
        //{
        //    Vector3 aux1 = vertices[1] - vertices[0];
        //    Vector3 aux2 = vertices[2] - vertices[0];
        //    float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
        //    print(crossProductZ);
        //    if (crossProductZ <= 0)//if they are clockwise swap them
        //    {
        //        Vector3 temp = vertices[1];
        //        vertices[1] = vertices[2];
        //        vertices[2] = temp;
        //    }
        //}
        //for (int i = 0; i < vertices.Count; i++)//join left to right
        //{
        //    hull.Add(vertices[i]);
        //    Debug.DrawLine(vertices[(i + 1)%vertices.Count], vertices[i], Color.blue, 99999999.9f);//Debugging purpouse to see the proccess 

        //}
        //return hull;
        return new List<Vector3> ();
    }

    bool CheckColinear(LinkedList<Vector3> vertices)
    {
        ////Ax * (By - Cy) + Bx * (Cy - Ay) + Cx * (Ay - By) / 2 check if the area of the triangle is 0, doesnt need to divide by 2
        //if (vertices.Count >= 3) return (vertices[0].x * (vertices[1].z - vertices[2].z) + vertices[1].x * (vertices[2].z - vertices[0].z) + vertices[2].x * (vertices[0].z - vertices[1].z)) == 0;
        ////when handling only 2 points catch the exception and send a true since a line will be created
        return true;
    }

    List<Vector3> Conquer(List<Vector3> hull1, List<Vector3> hull2)
    {
        List<Vector3> mergedHull = new List<Vector3>();
        Vector3 lRightmost = new Vector3();
        Vector3 rLeftmost = new Vector3();

        float aux1 = hull1.Max<Vector3>(p => p.x);
        lRightmost = hull1.FirstOrDefault(p => p.x == aux1);

        float aux2 = hull2.Min<Vector3>(p => p.x);
        rLeftmost = hull2.FirstOrDefault(p => p.x == aux2);

        //get the lowest tangent, check that there are no lowest values on both sides

        Vector3 low1 = CheckLowHeight(hull1, lRightmost);

        Vector3 low2= CheckLowHeight(hull2, rLeftmost);

        Debug.DrawLine(low1, low2, Color.green, 999999.9f);

        //get the highest tangent, check that there are no highest values on both sides

        Vector3 high1 = CheckHighHeight(hull1, lRightmost);

        Vector3 high2 = CheckHighHeight(hull2, rLeftmost);

        Debug.DrawLine(high1, high2, Color.cyan, 999999.9f);

        return mergedHull;
    }

    Vector3 CheckLowHeight(List<Vector3> a, Vector3 b)
    {
        Vector3 highest = b;
        for(int i = 0; i < a.Count; i++)
        {
            if (a[i].z < highest.z) highest = a[i];
        }
        return highest;
    }
    Vector3 CheckHighHeight(List<Vector3> a, Vector3 b)
    {
        Vector3 highest = b;
        for(int i = 0; i < a.Count; i++)
        {
            if (a[i].z > highest.z) highest = a[i];
        }
        return highest;
    }

}
