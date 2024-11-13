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
        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest
        QuickSort<Vector3>.Sort(vertices, 0, vertices.count - 1, (a, b) => new ComparerV().CompareX(a, b) < 0);
        //Divide and conquer algorithm
        Divide(0, vertices.count);
    }

    void Divide(int start, int end)
    {
        //i want to isolate points on groups of 2/3 to work individually and then join them
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        Hull merge = new Hull(start, end, vertices);

        Conquer(merge);//algorithm to join with the hull immediately to the left (and make the triangulation)
        Divide(end, vertices.count);//recursion to keep adding hulls step by step
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