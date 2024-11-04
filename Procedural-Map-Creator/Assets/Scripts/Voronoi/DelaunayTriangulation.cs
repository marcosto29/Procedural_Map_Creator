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
        vertices = new();
        //before doing the bisectors calculations, i need to do a delaunay triangulation to eliminate unnecessary triangles 
        Delaunay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Delaunay()//initial method that calculate the mediatrix between N points given
    {
        for (int i = 0; i < points; i++) vertices.Add(new Vector3(Random.Range(0, size.x), 0, Random.Range(0, size.y))); //random vertices position
        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest
        QuickSort<Vector3>.Sort(vertices, 0, vertices.count - 1, (a, b) => new ComparerV().Compare(a, b) < 0);//Sorting the List
        Divide(0, vertices.count);
    }

    void Divide(int start, int end)
    {
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        int edgeLimit = HullCreator(start, end);
        //Conquer(0, start - 1, start, edgeLimit);//algorithm to join with the hull immediately to the left (and make the triangulation, Conquer)
        Divide(edgeLimit, vertices.count);//recursion to keep adding hulls step by step
    }

    int HullCreator(int start, int end)
    {
        //we have the edge to create left and right side
        LinkedList<Vector3> aux = new LinkedList<Vector3>();

        for (int i = start; i < end ; i++) aux.Add(vertices[i]);
        //check wether the vertices are colinear to instead of drawing a triangle just join them
        bool line = Math.CheckColinear(aux);

        if (!line)
        {
            Vector3 aux1 = aux[1] - aux[0];
            Vector3 aux2 = aux[2] - aux[0];
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            print(crossProductZ);
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Vector3 temp = aux[1];
                aux[1] = aux[2];
                aux[2] = temp;
            }
        }
        for (int i = 0; i < aux.count; i++)//join left to right
        {
            Debug.DrawLine(aux[i], aux[(i + 1) % aux.count], Color.blue, 99999999.9f);//Debugging purpouse to see the proccess 
        }
        return end;
    }
    void Conquer(int start1, int end1, int start2, int end2)
    {
        LinkedList<Vector3> hull1 = new();
        LinkedList<Vector3> hull2 = new();
        for (int i = start1; i < end1; i++) hull1.Add(vertices[i]);
        for (int i = start2; i < end2; i++) hull2.Add(vertices[i]);
        Vector3 X = RM(hull1);
        Vector3 Y = LM(hull2);
        Vector3 Z = First(Y);
        Vector3 Z2 = First(X);
        Vector3 Z3 = Pred(X, Z2);
        if(X != Vector3.zero)
        {

        }

    }
    Vector3 RM(LinkedList<Vector3> auxV)
    {
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, (a, b) => new ComparerV().Compare(a, b) < 0);//Sorting the List
        if (auxV[0] == null) return Vector3.zero;
        return auxV[0];
    }
    Vector3 LM(LinkedList<Vector3> auxV)
    {
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, (a, b) => new ComparerV().Compare(a, b) > 0);//Sorting the List
        return auxV[0];
    }
    Vector3 First(Vector3 V)
    {
        return Vector3.zero;
    }
    Vector3 Pred(Vector3 V, Vector3 V2)
    {
        return Vector3.zero;
    }
    Vector3 Succ(Vector3 V, Vector3 V2)
    {
        return Vector3.zero;
    }
    bool IsRight(Vector3 LV, Vector3 LV2, Vector3 V)
    {
        return (LV2.x - V.x) * (V.y - LV.y) - (LV2.y - LV.y) * (V.x - LV.x) < 0;
    }
}
