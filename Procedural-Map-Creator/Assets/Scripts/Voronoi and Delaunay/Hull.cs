using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hull
{
    public List<Tuple<Node<Vector3>, Node<Vector3>>> edges;
    public List<Node<Vector3>> vertices;
    public List<Triangle> triangles;

    public Hull()
    {
        edges = new();
        vertices = new();
    }
    public Hull(int start, int end, List<Node<Vector3>> v)
    {
        edges = new();
        vertices = new();
        triangles = new();

        for (int i = start; i < end; i++) vertices.Add(v[i]);

        //check wether the vertices are colinear to instead of drawing a triangle just join them
        if (!Math.CheckColinear(vertices))
        {
            Vector3 aux1 = vertices[1].GetValue() - vertices[0].GetValue();
            Vector3 aux2 = vertices[2].GetValue() - vertices[0].GetValue();
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Node<Vector3> temp = vertices[1];
                int V2 = v.FindIndex(x => x == vertices[2]);
                int V = v.FindIndex(x => x == vertices[1]);

                v[V] = v[V2];
                v[V2] = temp;
                
                vertices[1] = vertices[2];//here too so that it can be later painted, this is for debugging purpose
                vertices[2] = temp;
            }
        }
        for (int i = 0; i < vertices.Count; i++)//join left to right
        {
            InsertEdge(vertices[i], vertices[(i + 1) % vertices.Count]);
            //Debug.DrawLine(points[i].GetValue(), points[(i + 1) % points.Count].GetValue(), Color.blue, 99999999.9f);//Debugging purpouse to see the proccess
        }
        if (vertices.Count >= 3) triangles.Add(new(vertices[0], vertices[1], vertices[2]));
    }

    public void InsertEdge(Node<Vector3> N1, Node<Vector3> N2)
    {
        AddAdjacency(N1, N2);
        AddAdjacency(N2, N1);

        if (!edges.Contains(new Tuple<Node<Vector3>, Node<Vector3>>(N1, N2)) && !edges.Contains(new Tuple<Node<Vector3>, Node<Vector3>>(N2, N1))) edges.Add(new Tuple<Node<Vector3>, Node<Vector3>>(N1, N2));
    }

    public void DeleteEdge(Node<Vector3> N1, Node<Vector3> N2)
    {
        Tuple <Node<Vector3>, Node<Vector3>> edge = new (N1, N2);
        Tuple <Node<Vector3>, Node<Vector3>> edge2 = new (N2, N1);
        edges.Remove(edge);
        edges.Remove(edge2);

        N1.GetAdjacency().Remove(N2);
        N2.GetAdjacency().Remove(N1);

        Triangle T = new();
        Triangle T2 = new();

        while (true)
        {
            
            T = triangles.Find(x => Array.Exists(x.edges, y => Math.TriangleComparer(y, edge)));
            T2 = triangles.Find(x => Array.Exists(x.edges, y => Math.TriangleComparer(y, edge2)));

            if (T == null && T2 == null)
                break;

            if (T != null) triangles.Remove(T);
            if (T2 != null) triangles.Remove(T2);
        }
    }

    void AddAdjacency(Node<Vector3> N1, Node<Vector3> N2)
    {
        float angle = Vector3.Angle(new Vector3(N1.GetValue().x, 0, N1.GetValue().z + 1) - N1.GetValue() , N2.GetValue() - N1.GetValue());//to order the adjacency points counter clockwise order calculate the angle with the X,Y edges created on the point
        if (!Math.IsRight(N1.GetValue(), new Vector3(N1.GetValue().x, 0, N1.GetValue().z + 1), N2.GetValue())) angle = 360 - angle;

        if (N1.GetAdjacency().Count == 0) N1.GetAdjacency().AddFirst(N2);
        else
        {
            foreach(Node<Vector3> i in N1.GetAdjacency())
            {
                float angle2 = Vector3.Angle(new Vector3(N1.GetValue().x, 0, N1.GetValue().z + 1) - N1.GetValue(), i.GetValue() - N1.GetValue());//to order the adjacency points counter clockwise order calculate the angle with the X,Y edges created on the point
                if (!Math.IsRight(N1.GetValue(), new Vector3(N1.GetValue().x, 0, N1.GetValue().z + 1), i.GetValue())) angle2 = 360 - angle2;
                
                if (angle2 > angle)
                {
                    if (!N1.GetAdjacency().Contains(N2)) N1.GetAdjacency().AddBefore(N1.GetAdjacency().Find(i), N2);
                    return;
                }
            }
            if(!N1.GetAdjacency().Contains(N2)) N1.GetAdjacency().AddLast(N2);
        }          
    }
}
