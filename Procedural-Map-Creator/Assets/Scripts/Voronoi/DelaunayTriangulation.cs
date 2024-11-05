using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
        for (int i = 0; i < points; i++) vertices.Add(new Vector3(UnityEngine.Random.Range(0, size.x), 0, UnityEngine.Random.Range(0, size.y))); //random vertices position
        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest
        QuickSort<Vector3>.Sort(vertices, 0, vertices.count - 1, (a, b) => new ComparerV().Compare(a, b) < 0);//Sorting the List
        Divide(0, vertices.count);
    }

    void Divide(int start, int end)
    {
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        int edgeLimit = HullCreator(start, end);
        Conquer(0, start - 1, start, edgeLimit);//algorithm to join with the hull immediately to the left (and make the triangulation, Conquer)
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
        Pred(aux[0], aux[0], aux);
        return end;
    }
    void Conquer(int start1, int end1, int start2, int end2)
    {
        LinkedList<Vector3> hull1 = new();
        LinkedList<Vector3> hull2 = new();
        for (int i = start1; i <= end1; i++) hull1.Add(vertices[i]);
        for (int i = start2; i < end2; i++) hull2.Add(vertices[i]);
        Vector3 X = RM(hull1);
        Vector3 Y = LM(hull2);
        Vector3 Z = First(Y, hull2);
        if(X != Vector3.zero)
        {
            Vector3 Z2 = First(X, hull1);
            Vector3 Z3 = Pred(X, Z2, hull1);
            while (!IsRight(X, Y, Z) && !IsRight(X, Y, Z3))
            {
                if (IsRight(X, Y, Z))
                {
                    Z = Succ(Z, Y, hull2);
                    Y = Z;
                }
                else
                {
                    if (IsRight(X, Y, Z3))
                    {
                        Z3 = Pred(Z3, X, hull1);
                        X = Z3;
                    }
                    else
                    {
                        Debug.DrawLine(X, Y, Color.green, 9999999.9f);
                    }
                }
            }
 
        }
    }
    Vector3 RM(LinkedList<Vector3> auxV)
    {
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, (a, b) => new ComparerV().Compare(a, b) > 0);//Sorting the List
        if (auxV.isEmpty()) return Vector3.zero;
        return auxV[0];
    }
    Vector3 LM(LinkedList<Vector3> auxV)
    {
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, (a, b) => new ComparerV().Compare(a, b) < 0);//Sorting the List
        return auxV[0];
    }
    Vector3 First(Vector3 V, LinkedList<Vector3> hull)
    {
        return hull.Get(V).GetSon().GetValue();
    }
    Vector3 Pred(Vector3 V, Vector3 V2, LinkedList<Vector3> hull)
    {
        LinkedList<Tuple<Vector3, float, float>> auxVectors = new();
        for (int i = 0; i < hull.count; i++) auxVectors.Add(new Tuple<Vector3, float, float>(hull[i], (V2.x - hull[i].x) * (hull[i].z - V.z) - (V2.z - V.z) * (hull[i].x - V.x), Math.Distance(V, V2, hull[i])));//the distance from a line to a point is the perpendicular
        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectors, 0, auxVectors.count - 1, (a, b) => new ComparerV().Compare(a.Item2, b.Item2, a.Item3, b.Item3) > 0);//sort them so that the ones to the right are added last (they are counter - clockwise ordered)
        return auxVectors[0].Item1;
    }
    Vector3 Succ(Vector3 V, Vector3 V2, LinkedList<Vector3> hull)
    {
        LinkedList<Tuple<Vector3, float, float>> auxVectors = new();
        for (int i = 0; i < hull.count; i++) auxVectors.Add(new Tuple<Vector3, float, float>(hull[i], (V2.x - hull[i].x) * (hull[i].z - V.z) - (V2.z - V.z) * (hull[i].x - V.x), Math.Distance(V, V2, hull[i])));//the distance from a line to a point is the perpendicular
        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectors, 0, auxVectors.count - 1, (a, b) => new ComparerV().Compare(a.Item2, b.Item2, a.Item3, b.Item3) < 0);//sort them so that the ones to the left are added last (they are clockwise ordered)
        return auxVectors[0].Item1;
    }
    bool IsRight(Vector3 LV, Vector3 LV2, Vector3 V)
    {
        return (LV2.x - V.x) * (V.z - LV.z) - (LV2.z - LV.z) * (V.x - LV.x) < 0; //check if the vertice is on the right with the help of cross product
    }
}
