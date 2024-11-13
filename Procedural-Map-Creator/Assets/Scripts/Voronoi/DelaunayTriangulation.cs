using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] LinkedList<Vector3> vertices;
    [SerializeField] Hull convexHull;
    // Start is called before the first frame update
    void Start()
    {
        convexHull = new();
        vertices = new();
        //before doing the bisectors calculations, i need to do a delaunay triangulation to eliminate unnecessary triangles 
        Delaunay();
    }
    void Delaunay()//initial method that calculate the mediatrix between N points given
    {
        //random vertices position
        for (int i = 0; i < points; i++) vertices.Add(new Vector3(UnityEngine.Random.Range(0, size.x), 0, UnityEngine.Random.Range(0, size.y)));

        //vertices[0] = new Vector3(2.65f, 0, 5.34f);
        //vertices[1] = new Vector3(4.44f, 0, 1.46f);
        //vertices[2] = new Vector3(4.78f, 0, 6.87f);
        //vertices[3] = new Vector3(6.92f, 0, 7.80f);
        //vertices[4] = new Vector3(8.88f, 0, 8.64f);
        //vertices[5] = new Vector3(9.72f, 0, 2.43f);
        //vertices[6] = new Vector3(9.96f, 0, 4.53f);

        //vertices[0] = new Vector3(2.04f, 0, 4.62f);
        //vertices[1] = new Vector3(3.95f, 0, 9.23f);
        //vertices[2] = new Vector3(4.34f, 0, 7.36f);
        //vertices[3] = new Vector3(5.25f, 0, 4.70f);
        //vertices[4] = new Vector3(6.08f, 0, 7.93f);
        //vertices[5] = new Vector3(7.04f, 0, 9.24f);
        //vertices[6] = new Vector3(8.26f, 0, 7.96f);
        //vertices[7] = new Vector3(9.47f, 0, 7.45f);
        //vertices[8] = new Vector3(9.84f, 0, 6.39f);

        //vertices[0] = new Vector3(0.45f, 0, 2.72f);
        //vertices[1] = new Vector3(1.65f, 0, 8.75f);
        //vertices[2] = new Vector3(2.37f, 0, 4.16f);
        //vertices[3] = new Vector3(2.67f, 0, 3.53f);
        //vertices[4] = new Vector3(4.13f, 0, 9.00f);
        //vertices[5] = new Vector3(6.16f, 0, 3.34f);
        //vertices[6] = new Vector3(8.02f, 0, 8.15f);
        //vertices[7] = new Vector3(8.18f, 0, 4.82f);
        //vertices[8] = new Vector3(9.79f, 0, 7.61f);


        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest
        QuickSort<Vector3>.Sort(vertices, 0, vertices.count - 1, (a, b) => new ComparerV().CompareX(a, b) < 0);//Sorting the List
        Divide(0, vertices.count);
    }

    void Divide(int start, int end)
    {
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        Tuple<int, Hull> merge = HullCreator(start, end);
        Conquer(merge.Item2);//algorithm to join with the hull immediately to the left (and make the triangulation, Conquer)
        Divide(merge.Item1, vertices.count);//recursion to keep adding hulls step by step
    }

    Tuple<int, Hull> HullCreator(int start, int end)
    {
        //we have the edge to create left and right side
        Hull aux = new();

        for (int i = start; i < end ; i++) aux.points.Add(vertices[i]);
        //check wether the vertices are colinear to instead of drawing a triangle just join them
        bool line = Math.CheckColinear(aux.points);

        if (!line)
        {
            Vector3 aux1 = aux.points[1] - aux.points[0];
            Vector3 aux2 = aux.points[2] - aux.points[0];
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            //((hull2[1] - hull2[0]).x * (hull2[2] - hull2[0]).z) - ((hull2[1] - hull2[0]).z * (hull2[2] - hull2[0]).x) 
            print(crossProductZ);
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Vector3 temp = aux.points[1];
                vertices.Get(aux.points[2]).SetValue(temp);
                vertices.Get(aux.points[1]).SetValue(aux.points[2]);//swap them on the array

                aux.points[1] = aux.points[2];//here too so that it can be later painted, this is for debugging purpose
                aux.points[2] = temp;
            }
        }
        for (int i = 0; i < aux.points.count; i++)//join left to right
        {
            aux.edgePoints.Add(aux.points[i]);
            Debug.DrawLine(aux.points[i], aux.points[(i + 1) % aux.points.count], Color.blue, 99999999.9f);//Debugging purpouse to see the proccess
            aux.edges.Add(new Tuple<Vector3, Vector3>(aux.points[i], aux.points[(i + 1) % aux.points.count]));
        }
        return new Tuple<int, Hull>(end, aux);
    }
    void Conquer(Hull mergingHull)
    {
        if(convexHull.points.count <= 0) convexHull = mergingHull;
        else
        {
            Tuple<Vector3, Vector3> lowTangent = LowTangent(mergingHull);
            Tuple<Vector3, Vector3> highTangent = HighTangent(mergingHull);
            convexHull.points.Print();
            mergingHull.points.Print();
            JoinHulls(mergingHull, lowTangent, highTangent);
        }
    }
    Tuple<Vector3, Vector3> LowTangent(Hull mergingHull)
    {
        Vector3 X = Math.ChosenPoint(new LinkedList<Vector3>(convexHull.points), (a, b) => new ComparerV().CompareX(a, b) > 0);
        Vector3 Y = Math.ChosenPoint(new LinkedList<Vector3>(mergingHull.points), (a, b) => new ComparerV().CompareX(a, b) < 0);

        Vector3 centerX = X;
        Vector3 centerY = Y;

        Vector3 Z2 = convexHull.FollowingPoint(centerX, convexHull.edgePoints.Get(X).GetSon().GetValue(), "Right");
        Vector3 Z = mergingHull.FollowingPoint(centerY, mergingHull.edgePoints.Get(Y).GetFather().GetValue(), "Left");

        while (Math.IsRight(X, Y, Z) == true || Math.IsRight(X, Y, Z2) == true)
        {
            if (Math.IsRight(X, Y, Z))
            {
                Y = Z;
                Z = mergingHull.FollowingPoint(centerY, Y, "Left");
            }
            else
            {
                if (Math.IsRight(X, Y, Z2))
                {
                    X = Z2;
                    Z2 = convexHull.FollowingPoint(centerX, X, "Right");                  
                }
            }
        }
        Debug.DrawLine(X, Y, Color.green, 9999999.9f);
        return new Tuple<Vector3, Vector3>(X, Y);
    }
    Tuple<Vector3, Vector3> HighTangent(Hull mergingHull)
    {
        Vector3 X = Math.ChosenPoint(new LinkedList<Vector3>(convexHull.points), (a, b) => new ComparerV().CompareX(a, b) > 0);
        Vector3 Y = Math.ChosenPoint(new LinkedList<Vector3>(mergingHull.points), (a, b) => new ComparerV().CompareX(a, b) < 0);

        Vector3 centerX = X;
        Vector3 centerY = Y;

        Vector3 Z2 = convexHull.FollowingPoint(centerX, mergingHull.edgePoints.Get(Y).GetFather().GetValue(), "Left");
        Vector3 Z = mergingHull.FollowingPoint(centerY, mergingHull.edgePoints.Get(Y).GetSon().GetValue(), "Right");

        while ((!Math.IsRight(X, Y, Z) && Z != Y) || (!Math.IsRight(X, Y, Z2) && X != Z2))
        {
            if (!Math.IsRight(X, Y, Z) && Z != Y)
            {
                Y = Z;
                Z = mergingHull.FollowingPoint(centerY, Y, "Right");
            }
            else
            {
                if (!Math.IsRight(X, Y, Z2) && X != Z2)
                {
                    X = Z2;
                    Z2 = convexHull.FollowingPoint(centerX, X, "Left");
                }
            }
        }
        Debug.DrawLine(X, Y, Color.magenta, 9999999.9f);
        return new Tuple<Vector3, Vector3>(X, Y);
    }
    void JoinHulls(Hull mergingHull, Tuple<Vector3, Vector3> low, Tuple<Vector3, Vector3> high)
    {
        //join edges, edgePoints and points
        Node<Vector3> low1 = convexHull.edgePoints.Get(low.Item1);
        Node<Vector3> low2 = mergingHull.edgePoints.Get(low.Item2);
        Node<Vector3> high1 = convexHull.edgePoints.Get(high.Item1);
        Node<Vector3> high2 = mergingHull.edgePoints.Get(high.Item2);

        low1.SetSon(low2);
        low2.SetFather(low1);

        high1.SetFather(high2);
        high2.SetSon(high1);

        convexHull.edgePoints.count = 1;
        Node<Vector3> endPoint = low1.GetFather();
        while (low1 != endPoint)
        {
            convexHull.edgePoints.count++;
            low1 = low1.GetSon();
        }

        for (int i = 0; i < mergingHull.points.count; i++)
        {
            convexHull.points.Add(mergingHull.points[i]);
        }
    }
}