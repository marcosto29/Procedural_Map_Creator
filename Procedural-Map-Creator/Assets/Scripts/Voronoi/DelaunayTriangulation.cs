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

        //vertices[0] = new Vector3(.01f, 0, 2.23f);
        //vertices[1] = new Vector3(.71f, 0, 1.89f);
        //vertices[2] = new Vector3(.91f, 0, 8.88f);
        //vertices[3] = new Vector3(7.61f, 0, 1.45f);
        //vertices[4] = new Vector3(6.90f, 0, 5.56f);

        vertices[0] = new Vector3(0.46f, 0, 5.99f);
        vertices[1] = new Vector3(1.61f, 0, 8.99f);
        vertices[2] = new Vector3(2.47f, 0, 3.75f);
        vertices[3] = new Vector3(4.45f, 0, 4.56f);
        vertices[4] = new Vector3(8.20f, 0, 2.78f);

        QuickSort<Vector3>.Sort(vertices, 0, vertices.count - 1, (a, b) => new ComparerV().CompareX(a, b) < 0);
        //Divide and conquer algorithm
        Divide(0, vertices.count);
    }

    void Divide(int start, int end)
    {
        //i want to isolate points on groups of 2/3 to work individually and then join them
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        Hull merge = new (start, end, vertices);

        Conquer(merge);//algorithm to join with the hull immediately to the left (and make the triangulation)
        Divide(end, vertices.count);//recursion to keep adding hulls step by step
    }
    void Conquer(Hull mergingHull)
    {
        if(convexHull.points.count <= 0) convexHull = mergingHull;
        else
        {
            Vector3 X = Math.ChosenPoint(new LinkedList<Vector3>(convexHull.points), (a, b) => new ComparerV().CompareX(a, b) > 0);
            Vector3 Y = Math.ChosenPoint(new LinkedList<Vector3>(mergingHull.points), (a, b) => new ComparerV().CompareX(a, b) < 0);

            Vector3 centerX = X;
            Vector3 centerY = Y;

            Tuple<Vector3, Vector3> lowTangent = LowTangent(mergingHull, centerX, centerY, X, Y);
            Tuple<Vector3, Vector3> highTangent = HighTangent(mergingHull, centerX, centerY, X, Y);

            JoinHulls(mergingHull, lowTangent, highTangent);

            for(int i = 0; i < convexHull.edges.Count; i++) Debug.DrawLine(convexHull.edges[i].Item1, convexHull.edges[i].Item2, Color.blue, 9999999.9f);
        }
    }
    Tuple<Vector3, Vector3> LowTangent(Hull mergingHull, Vector3 centerX, Vector3 centerY, Vector3 X, Vector3 Y)
    {

        Vector3 Z2 = convexHull.FollowingPoint(centerX, convexHull.edgePoints.Get(X).GetSon(), "Right"); //travesing through the boundary CW direction
        Vector3 Z = mergingHull.FollowingPoint(centerY, mergingHull.edgePoints.Get(Y).GetFather(), "Left"); //travesing through the boundary CCW direction

        while (Math.IsRight(X, Y, Z) || Math.IsRight(X, Y, Z2))
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
        //Debug.DrawLine(X, Y, Color.green, 9999999.9f);
        return new Tuple<Vector3, Vector3>(X, Y);
    }
    Tuple<Vector3, Vector3> HighTangent(Hull mergingHull, Vector3 centerX, Vector3 centerY, Vector3 X, Vector3 Y)
    {
        Vector3 Z2 = convexHull.FollowingPoint(centerX, convexHull.edgePoints.Get(X).GetFather(), "Left"); //travesing through the boundary CCW direction
        Vector3 Z = mergingHull.FollowingPoint(centerY, mergingHull.edgePoints.Get(Y).GetSon(), "Right"); //travesing through the boundary CW direction

        while (Math.IsLeft(X, Y, Z) || Math.IsLeft(X, Y, Z2))
        {
            if (Math.IsLeft(X, Y, Z))
            {
                Y = Z;
                Z = mergingHull.FollowingPoint(centerY, Y, "Right");
            }
            else
            {
                if (Math.IsLeft(X, Y, Z2))
                {
                    X = Z2;
                    Z2 = convexHull.FollowingPoint(centerX, X, "Left");
                }
            }
        }
        //Debug.DrawLine(X, Y, Color.magenta, 9999999.9f);
        return new Tuple<Vector3, Vector3>(X, Y);
    }
    void JoinHulls(Hull mergingHull, Tuple<Vector3, Vector3> low, Tuple<Vector3, Vector3> high)//merge the hulls with with the delaunay condition
    {
        for (int i = 0; i < mergingHull.points.count; i++) convexHull.points.Add(mergingHull.points[i]); //Add all points to the final Hull
        for (int i = 0; i < mergingHull.edges.Count; i++) convexHull.edges.Add(mergingHull.edges[i]); //Add all points to the final Hull

        Tuple<Vector3, Vector3> aux = low;

        Vector3 L = aux.Item1;
        Vector3 R = aux.Item2;
        while (!aux.Equals(high))
        {
            bool A = false, B = false;
            convexHull.InsertEdge(new Tuple<Vector3, Vector3>(L, R)); //Add a Delaunay Edge to the Hull
            Vector3 R1 = mergingHull.FollowingPoint(R, L, "Right");
            if (Math.IsLeft(L, R, R1)) //check on the right hull the next possible vertex
            {
                Vector3 R2 = mergingHull.FollowingPoint(R, R1, "Right"); //take a Q point on the right side to check the circumcircle
                while (!Math.QTest(R1, L, R, R2))//while the circumcircle rule is not accepted search new points
                {
                    convexHull.DeleteEdge(new Tuple<Vector3, Vector3>(R, R1));
                    R1 = R2;
                    R2 = mergingHull.FollowingPoint(R, R1, "Right");
                }
            }
            else A = true;
            Vector3 L1 = convexHull.FollowingPoint(L, R, "Left");
            if (Math.IsRight(R, L, L1))
            {
                Vector3 L2 = convexHull.FollowingPoint(L, L1, "Left");
                while (!Math.QTest(L, R, L1, L2))
                {
                    convexHull.DeleteEdge(new Tuple<Vector3, Vector3>(L, L1));//remove this edge
                    L1 = L2;
                    L2 = convexHull.FollowingPoint(L, L1, "Left");
                }
            }
            else B = true;
            if (A) L = L1;
            else
            {
                if (B) R = R1;
                else
                {
                    if (Math.QTest(L, R, R1, L1)) R = R1;
                    else L = L1;
                }
            }
            aux = new Tuple<Vector3, Vector3>(L, R);
        }
        convexHull.InsertEdge(new Tuple<Vector3, Vector3>(L, R)); //Insert the upper tangent


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
    }
}
