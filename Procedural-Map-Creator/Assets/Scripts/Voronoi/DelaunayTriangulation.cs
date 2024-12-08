using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Diagnostics;
using System.IO;

public class DelaunayTriangulation : MonoBehaviour
{
    [SerializeField] Vector2 size;
    [SerializeField] int points;
    [SerializeField] List<Node<Vector3>> vertices;
    [SerializeField] Hull convexHull;
    private string filePath;
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
        QuickSort<Node<Vector3>>.Sort(vertices, 0, vertices.Count - 1, (a, b) => new ComparerV().CompareX(a.GetValue(), b.GetValue()) < 0);

        for (int i = 0; i < vertices.Count; i++) System.Diagnostics.Debug.WriteLine(vertices[i].GetValue().x + " " + vertices[i].GetValue().z);
        for (int i = 0; i < vertices.Count; i++) print(vertices[i].GetValue().x + " " + vertices[i].GetValue().z);

        filePath = Path.Combine(Application.persistentDataPath, "SavedNumbers.txt");

        // Save the numbers
        SaveNumbers(vertices);

        //Divide and conquer algorithm
        Divide(0, vertices.Count);

        for (int i = 0; i < convexHull.edges.Count; i++) UnityEngine.Debug.DrawLine(convexHull.edges[i].Item1.GetValue(), convexHull.edges[i].Item2.GetValue(), Color.blue, 9999999.9f);
        for (int i = 0; i < convexHull.points.Count; i++) print(convexHull.points[i].GetValue());
    }

    void SaveNumbers(List<Node<Vector3>> numbers)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (Node<Vector3> number in numbers)
            {
                writer.WriteLine(number.GetValue().ToString("F5")); // Save numbers with five decimal places
            }
        }
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

        Node<Vector3> Z2 = FollowingPoint(X, convexHull.points.Find(x => x == X).GetSon(), "Right"); //travesing through the boundary CW direction
        Node<Vector3> Z = FollowingPoint(Y, mergingHull.points.Find(y => y == Y).GetFather(), "Left"); //travesing through the boundary CCW direction

        while (Math.IsRight(X.GetValue(), Y.GetValue(), Z.GetValue()) || Math.IsRight(X.GetValue(), Y.GetValue(), Z2.GetValue()))
        {
            if (Math.IsRight(X.GetValue(), Y.GetValue(), Z.GetValue()))
            {
                Node<Vector3> OldY = Y;
                Y = Z;
                Z = FollowingPoint(Y, OldY, "Left");
            }
            else
            {
                if (Math.IsRight(X.GetValue(), Y.GetValue(), Z2.GetValue()))
                {
                    Node<Vector3> OldX = X;
                    X = Z2;
                    Z2 = FollowingPoint(X, OldX, "Right");                  
                }
            }
        }
        //Debug.DrawLine(X.GetValue(), Y.GetValue(), Color.green, 9999999.9f);
        return new Tuple<Node<Vector3>, Node<Vector3>>(X, Y);
    }
    Tuple<Node<Vector3>, Node<Vector3>> HighTangent(Hull mergingHull, Node<Vector3> X, Node<Vector3> Y)
    {
        Node<Vector3> Z2 = FollowingPoint(X, convexHull.points.Find(x => x == X).GetFather(), "Left"); //travesing through the boundary CCW direction
        Node<Vector3> Z = FollowingPoint(Y, mergingHull.points.Find(x => x == Y).GetSon(), "Right"); //travesing through the boundary CW direction

        while (Math.IsLeft(X.GetValue(), Y.GetValue(), Z.GetValue()) || Math.IsLeft(X.GetValue(), Y.GetValue(), Z2.GetValue()))
        {
            if (Math.IsLeft(X.GetValue(), Y.GetValue(), Z.GetValue()))
            {
                Node<Vector3> OldY = Y;
                Y = Z;
                Z = FollowingPoint(Y, OldY, "Right");
            }
            else
            {
                if (Math.IsLeft(X.GetValue(), Y.GetValue(), Z2.GetValue()))
                {
                    Node<Vector3> OldX = X;
                    X = Z2;
                    Z2 = FollowingPoint(X, OldX, "Left");
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
            Node<Vector3> R1 = FollowingPoint(R, L, "Right");
            if (Math.IsLeft(L.GetValue(), R.GetValue(), R1.GetValue())) //check on the right hull the next possible vertex
            {
                Node<Vector3> R2 = FollowingPoint(R, R1, "Right"); //take a Q point on the right side to check the circumcircle
                while (!Math.QTest(R1.GetValue(), L.GetValue(), R.GetValue(), R2.GetValue()))//while the circumcircle rule is not accepted search new points
                {
                    convexHull.DeleteEdge(R, R1);
                    R1 = R2;
                    R2 = FollowingPoint(R, R1, "Right");
                }
            }
            else A = true;
            Node<Vector3> L1 = FollowingPoint(L, R, "Left");
            if (Math.IsRight(R.GetValue(), L.GetValue(), L1.GetValue()))
            {
                Node<Vector3> L2 = FollowingPoint(L, L1, "Left");
                while (!Math.QTest(L.GetValue(), R.GetValue(), L1.GetValue(), L2.GetValue()))
                {
                    convexHull.DeleteEdge(L, L1);//remove this edge
                    L1 = L2;
                    L2 = FollowingPoint(L, L1, "Left");
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

    public Node<Vector3> FollowingPoint(Node<Vector3> V, Node<Vector3> V2, string sequence)
    {
        if (V.GetAdjacency().Count == 1) return V.GetAdjacency().First.Value;//segment case

        //create an aux list that will contain each point with the distance to the segment and whether is on the right or left side
        List<Tuple<Node<Vector3>, bool, float>> auxVectors = new();

        foreach (Node<Vector3> i in V.GetAdjacency()) //cut the sample points to the locals given the point V
        {
            bool isRight = Math.IsRight(V.GetValue(), V2.GetValue(), i.GetValue());
            float angle = Vector3.Angle(i.GetValue() - V.GetValue(), V2.GetValue() - V.GetValue());//calculate the angle and if the point is on the right or left side of the segmente
            if ((sequence == "Left" && isRight) || (sequence == "Right" && !isRight))// since Vector3.angle only gives a number between 0 and 180 the case where the angle is concave needs to be treated
            {
                angle = 360 - angle;
            }

            if (i != V2 && i != V) auxVectors.Add(new Tuple<Node<Vector3>, bool, float>(i, isRight, angle));//filter the point of the hull being checked
        }
        //sort them from closest to farthest
        QuickSort<Tuple<Node<Vector3>, bool, float>>.Sort(auxVectors, 0, auxVectors.Count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort the points to know which one is the closest to the one being cheked 
        if (auxVectors.Count == 0)
        {
            print(V.GetValue() + "im not working");
        }
        return auxVectors[0].Item1;
    }
}
