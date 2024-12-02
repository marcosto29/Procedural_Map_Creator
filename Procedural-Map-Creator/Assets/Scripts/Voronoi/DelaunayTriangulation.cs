using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] List<Node<Vector3>> vertices;
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
        for (int i = 0; i < points; i++) vertices.Add(new Node<Vector3>(new Vector3(UnityEngine.Random.Range(0, size.x), 0, UnityEngine.Random.Range(0, size.y))));
        //sort them on a lexicographically ascending order (comparing first the x-coordinates and if its the same value the y-coordinate) from lowest to highest

        //vertices[0] = new Vector3(.01f, 0, 2.23f);
        //vertices[1] = new Vector3(.71f, 0, 1.89f);
        //vertices[2] = new Vector3(.91f, 0, 8.88f);
        //vertices[3] = new Vector3(7.61f, 0, 1.45f);
        //vertices[4] = new Vector3(6.90f, 0, 5.56f);

        //vertices[1] = new Node<Vector3>(new Vector3(0.46f, 0, 5.99f));
        //vertices[0] = new Node<Vector3>(new Vector3(1.61f, 0, 8.99f));
        //vertices[4] = new Node<Vector3>(new Vector3(2.47f, 0, 3.75f));
        //vertices[3] = new Node<Vector3>(new Vector3(4.45f, 0, 4.56f));
        //vertices[2] = new Node<Vector3>(new Vector3(8.20f, 0, 2.78f));

        //vertices[0] = new Node<Vector3>(new Vector3(2.6f, 0, 7f));
        //vertices[1] = new Node<Vector3>(new Vector3(1.61f, 0, 0f));
        //vertices[2] = new Node<Vector3>(new Vector3(4.8f, 0, 5.2f));
        //vertices[3] = new Node<Vector3>(new Vector3(9.1f, 0, 7.3f));
        //vertices[4] = new Node<Vector3>(new Vector3(9.15f, 0, 6.3f));
        //vertices[5] = new Node<Vector3>(new Vector3(9.2f, 0, 7.9f));
        //vertices[6] = new Node<Vector3>(new Vector3(9.2f, 0, 7.9f));

        //vertices[0] = new Node<Vector3>(new Vector3(2.5f, 0, 0.2f));
        //vertices[1] = new Node<Vector3>(new Vector3(2f, 0, 6.6f));
        //vertices[2] = new Node<Vector3>(new Vector3(4.2f, 0, 4.8f));
        //vertices[3] = new Node<Vector3>(new Vector3(5.7f, 0, 7.3f));
        //vertices[4] = new Node<Vector3>(new Vector3(7.4f, 0, 0.8f));
        //vertices[5] = new Node<Vector3>(new Vector3(9.1f, 0, 9.3f));
        //vertices[6] = new Node<Vector3>(new Vector3(9.7f, 0, 9.1f));

        QuickSort<Node<Vector3>>.Sort(vertices, 0, vertices.Count - 1, (a, b) => new ComparerV().CompareX(a.GetValue(), b.GetValue()) < 0);
        //Divide and conquer algorithm
        Divide(0, vertices.Count);

        for (int i = 0; i < convexHull.edges.Count; i++) Debug.DrawLine(convexHull.edges[i].Item1.GetValue(), convexHull.edges[i].Item2.GetValue(), Color.blue, 9999999.9f);
        for (int i = 0; i < convexHull.points.Count; i++) print(convexHull.points[i].GetValue());
    }

    void Divide(int start, int end)
    {
        //i want to isolate points on groups of 2/3 to work individually and then join them
        if (end <= start) return;

        while (end - start > 3) end = ((end - start) / 2) + start;//formula to navigate through vertices

        Hull merge = new (start, end, vertices);

        Conquer(merge);//algorithm to join with the hull immediately to the left (and make the triangulation)
        Divide(end, vertices.Count);//recursion to keep adding hulls step by step
    }
    void Conquer(Hull mergingHull)
    {
        if(convexHull.points.Count <= 0) convexHull = mergingHull;
        else
        {
            Node<Vector3> X = convexHull.points.Find(x => x.GetValue() == Math.ChosenPoint(convexHull.points.Select(x => x.GetValue()).ToList(), (a, b) => new ComparerV().CompareX(a, b) > 0));
            Node<Vector3> Y = mergingHull.points.Find(y => y.GetValue() == Math.ChosenPoint(mergingHull.points.Select(y => y.GetValue()).ToList(), (a, b) => new ComparerV().CompareX(a, b) < 0));

            Tuple<Node<Vector3>, Node<Vector3>> lowTangent = LowTangent(mergingHull, X, Y);
            Tuple<Node<Vector3>, Node<Vector3>> highTangent = HighTangent(mergingHull, X, Y);

            JoinHulls(mergingHull, lowTangent, highTangent);
        }
    }
    Tuple<Node<Vector3>, Node<Vector3>> LowTangent(Hull mergingHull, Node<Vector3> X, Node<Vector3> Y)
    {

        Node<Vector3> Z2 = convexHull.FollowingPoint(X, convexHull.points.Find(x => x == X).GetSon(), "Right"); //travesing through the boundary CW direction
        Node<Vector3> Z = mergingHull.FollowingPoint(Y, mergingHull.points.Find(y => y == Y).GetFather(), "Left"); //travesing through the boundary CCW direction

        while (Math.IsRight(X.GetValue(), Y.GetValue(), Z.GetValue()) || Math.IsRight(X.GetValue(), Y.GetValue(), Z2.GetValue()))
        {
            if (Math.IsRight(X.GetValue(), Y.GetValue(), Z.GetValue()))
            {
                Node<Vector3> OldY = Y;
                Y = Z;
                Z = mergingHull.FollowingPoint(Y, OldY, "Left");
            }
            else
            {
                if (Math.IsRight(X.GetValue(), Y.GetValue(), Z2.GetValue()))
                {
                    Node<Vector3> OldX = X;
                    X = Z2;
                    Z2 = convexHull.FollowingPoint(X, OldX, "Right");                  
                }
            }
        }
        //Debug.DrawLine(X.GetValue(), Y.GetValue(), Color.green, 9999999.9f);
        return new Tuple<Node<Vector3>, Node<Vector3>>(X, Y);
    }
    Tuple<Node<Vector3>, Node<Vector3>> HighTangent(Hull mergingHull, Node<Vector3> X, Node<Vector3> Y)
    {
        Node<Vector3> Z2 = convexHull.FollowingPoint(X, convexHull.points.Find(x => x == X).GetFather(), "Left"); //travesing through the boundary CCW direction
        Node<Vector3> Z = mergingHull.FollowingPoint(Y, mergingHull.points.Find(x => x == Y).GetSon(), "Right"); //travesing through the boundary CW direction

        while (Math.IsLeft(X.GetValue(), Y.GetValue(), Z.GetValue()) || Math.IsLeft(X.GetValue(), Y.GetValue(), Z2.GetValue()))
        {
            if (Math.IsLeft(X.GetValue(), Y.GetValue(), Z.GetValue()))
            {
                Node<Vector3> OldY = Y;
                Y = Z;
                Z = mergingHull.FollowingPoint(Y, OldY, "Right");
            }
            else
            {
                if (Math.IsLeft(X.GetValue(), Y.GetValue(), Z2.GetValue()))
                {
                    Node<Vector3> OldX = X;
                    X = Z2;
                    Z2 = convexHull.FollowingPoint(X, OldX, "Left");
                }
            }
        }
        //Debug.DrawLine(X.GetValue(), Y.GetValue(), Color.magenta, 9999999.9f);
        return new Tuple<Node<Vector3>, Node<Vector3>>(X, Y);
    }
    void JoinHulls(Hull mergingHull, Tuple<Node<Vector3>, Node<Vector3>> low, Tuple<Node<Vector3>, Node<Vector3>> high)//merge the hulls with with the delaunay condition
    {
        convexHull.edges.AddRange(mergingHull.edges);//Add the edges to the Hull

        Node<Vector3> L = low.Item1;
        Node<Vector3> R = low.Item2;

        while (!low.Equals(high))
        {
            bool A = false, B = false;
            convexHull.InsertEdge(L, R); //Add a Delaunay Edge to the Hull
            Node<Vector3> R1 = mergingHull.FollowingPoint(R, L, "Right");
            if (Math.IsLeft(L.GetValue(), R.GetValue(), R1.GetValue())) //check on the right hull the next possible vertex
            {
                Node<Vector3> R2 = mergingHull.FollowingPoint(R, R1, "Right"); //take a Q point on the right side to check the circumcircle
                while (!Math.QTest(R1.GetValue(), L.GetValue(), R.GetValue(), R2.GetValue()))//while the circumcircle rule is not accepted search new points
                {
                    convexHull.DeleteEdge(R, R1);
                    R1 = R2;
                    R2 = mergingHull.FollowingPoint(R, R1, "Right");
                }
            }
            else A = true;
            Node<Vector3> L1 = convexHull.FollowingPoint(L, R, "Left");
            if (Math.IsRight(R.GetValue(), L.GetValue(), L1.GetValue()))
            {
                Node<Vector3> L2 = convexHull.FollowingPoint(L, L1, "Left");
                while (!Math.QTest(L.GetValue(), R.GetValue(), L1.GetValue(), L2.GetValue()))
                {
                    convexHull.DeleteEdge(L, L1);//remove this edge
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
                    if (Math.QTest(L.GetValue(), R.GetValue(), R1.GetValue(), L1.GetValue())) R = R1;
                    else L = L1;
                }
            }
            low = new Tuple<Node<Vector3>, Node<Vector3>>(L, R);
        }
        convexHull.InsertEdge(L, R); //Insert the upper tangent
        convexHull.points.AddRange(mergingHull.points); //Add the points to the hull
    }
}
