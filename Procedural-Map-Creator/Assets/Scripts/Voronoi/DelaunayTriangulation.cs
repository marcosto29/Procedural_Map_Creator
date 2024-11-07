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
        if(start - 1 > 0) Conquer(0, start - 1, start, edgeLimit);//algorithm to join with the hull immediately to the left (and make the triangulation, Conquer)
        Divide(edgeLimit, vertices.count);//recursion to keep adding hulls step by step
    }

    int HullCreator(int start, int end)
    {
        //we have the edge to create left and right side
        LinkedList<Vector3> aux = new();

        for (int i = start; i < end ; i++) aux.Add(vertices[i]);
        //check wether the vertices are colinear to instead of drawing a triangle just join them
        bool line = Math.CheckColinear(aux);

        if (!line)
        {
            Vector3 aux1 = aux[1] - aux[0];
            Vector3 aux2 = aux[2] - aux[0];
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            //((hull2[1] - hull2[0]).x * (hull2[2] - hull2[0]).z) - ((hull2[1] - hull2[0]).z * (hull2[2] - hull2[0]).x) 
            print(crossProductZ);
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Vector3 temp = aux[1];
                vertices.Get(aux[2]).SetValue(temp);
                vertices.Get(aux[1]).SetValue(aux[2]);//swap them on the array

                aux[1] = aux[2];//here too so that it can be later painted, this is for debugging purpose
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
        for (int i = start1; i <= end1; i++) hull1.Add(vertices[i]);
        for (int i = start2; i < end2; i++) hull2.Add(vertices[i]);

        Vector3 X = ChosenPoint(new LinkedList<Vector3>(hull1), (a, b) => new ComparerV().Compare(a, b) > 0);
        Vector3 Y = ChosenPoint(new LinkedList<Vector3>(hull2), (a, b) => new ComparerV().Compare(a, b) < 0);

        Vector3 Z = First(Y, hull2);
        Vector3 Z2 = First(X, hull1);
        Vector3 Z3 = Pred(X, Z2, hull1);
        while (IsRight(X, Y, Z) == true || IsRight(X, Y, Z3) == true)
        {
            if (IsRight(X, Y, Z))
            {
                //first the new Y is the actual Z
                // to calculate the new Z, Succ(Z, Y) being Y the previous one
                Vector3 oldZ = Z;
                Z = Succ(Z, Y, hull2);
                Y = oldZ;
            }
            else
            {
                //first the new Y is the actual Z
                // to calculate the new Z, Succ(Z, Y) being Y the previous one
                if (IsRight(X, Y, Z3))
                {
                    Vector3 oldZ3 = Z3;
                    Z3 = Pred(Z3, X, hull1);
                    X = oldZ3;
                }
            }
        }
        Debug.DrawLine(X, Y, Color.green, 9999999.9f);
    }
    Vector3 ChosenPoint(LinkedList<Vector3> auxV, Func<Vector3, Vector3, bool> comparer)
    {    
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, comparer);//Sorting the List
        return auxV[0];
    }
    Vector3 First(Vector3 V, LinkedList<Vector3> hull)//get the first vertice counter - clockwise, since is already sorted that way, the son
    {
        return hull.Get(V).GetSon().GetValue();
    }
    Vector3 Pred(Vector3 V, Vector3 V2, LinkedList<Vector3> hull)//this method returns the closest value clockwise
    {
        if (hull.count == 2) return (hull[0] == V) ? hull[1] : hull[0];

        LinkedList<Tuple<Vector3, float, float>> auxVectors = new();
        for (int i = 0; i < hull.count; i++) auxVectors.Add(new Tuple<Vector3, float, float>(hull[i], (V2.x - hull[i].x) * (hull[i].z - V.z) - (V2.z - V.z) * (hull[i].x - V.x), Math.Distance(V, V2, hull[i])));//the distance from a line to a point is the perpendicular

        LinkedList<Tuple<Vector3, float, float>> auxVectorsNegatives = new();
        LinkedList<Tuple<Vector3, float, float>> auxVectorsPositives = new();

        for (int i = 0; i < auxVectors.count; i++)
        {
            if (auxVectors[i].Item2 < 0 && auxVectors[i].Item3 != 0) auxVectorsNegatives.Add(auxVectors[i]);
            else if (auxVectors[i].Item2 > 0 && auxVectors[i].Item3 != 0) auxVectorsPositives.Add(auxVectors[i]);
        }

        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectorsNegatives, 0, auxVectorsNegatives.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort them so that the ones to the left are added last (they are clockwise ordered)
        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectorsPositives, 0, auxVectorsPositives.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort them so that the ones to the left are added last (they are clockwise ordered)

        for (int i = 0; i < auxVectorsPositives.count; i++)
        {
            auxVectorsNegatives.Add(auxVectorsPositives[i]);
        } 

        return auxVectorsNegatives[0].Item1;
    }
    Vector3 Succ(Vector3 V, Vector3 V2, LinkedList<Vector3> hull)//this method returns the closest value counter - clockwise
    {
        if (hull.count == 2) return (hull[0] == V) ? hull[1] : hull[0];

        LinkedList<Tuple<Vector3, float, float>> auxVectors = new();
        for (int i = 0; i < hull.count; i++) auxVectors.Add(new Tuple<Vector3, float, float>(hull[i], (V2.x - hull[i].x) * (hull[i].z - V.z) - (V2.z - V.z) * (hull[i].x - V.x), Math.Distance(V, V2, hull[i])));//the distance from a line to a point is the perpendicular

        LinkedList<Tuple<Vector3, float, float>> auxVectorsNegatives = new();
        LinkedList<Tuple<Vector3, float, float>> auxVectorsPositives = new();

        for (int i = 0; i < auxVectors.count; i++)
        {
            if (auxVectors[i].Item2 < 0 && auxVectors[i].Item3 != 0) auxVectorsNegatives.Add(auxVectors[i]);
            else if (auxVectors[i].Item2 > 0 && auxVectors[i].Item3 != 0) auxVectorsPositives.Add(auxVectors[i]);
        }

        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectorsNegatives, 0, auxVectorsNegatives.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort them so that the ones to the left are added last (they are clockwise ordered)
        QuickSort<Tuple<Vector3, float, float>>.Sort(auxVectorsPositives, 0, auxVectorsPositives.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort them so that the ones to the left are added last (they are clockwise ordered)

        for (int i = 0; i < auxVectorsNegatives.count; i++)
        {
            auxVectorsPositives.Add(auxVectorsNegatives[i]);
        }

        return auxVectorsPositives[0].Item1;
    }
    bool IsRight(Vector3 A, Vector3 B, Vector3 M)
    {
        //AB AM
        return (A.x - B.x) * (M.z - B.z) - (A.z - B.z) * (M.x - B.x) > 0; //check if the vertice is on the right with the help of cross product
    }
}
