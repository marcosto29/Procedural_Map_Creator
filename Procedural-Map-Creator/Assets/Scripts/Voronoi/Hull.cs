using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hull
{
    public List<Tuple<Node<Vector3>, Node<Vector3>>> edges;
    public List<Node<Vector3>> points;

    public Hull()
    {
        edges = new();
        points = new();
    }
    public Hull(int start, int end, List<Node<Vector3>> vertices)
    {
        edges = new();
        points = new();

        for (int i = start; i < end; i++) points.Add(vertices[i]);

        //check wether the vertices are colinear to instead of drawing a triangle just join them
        if (!Math.CheckColinear(points))
        {
            Vector3 aux1 = points[1].GetValue() - points[0].GetValue();
            Vector3 aux2 = points[2].GetValue() - points[0].GetValue();
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Node<Vector3> temp = points[1];
                int V2 = vertices.FindIndex(x => x == points[2]);
                int V = vertices.FindIndex(x => x == points[1]);

                vertices[V] = vertices[V2];
                vertices[V2] = temp;
                
                points[1] = points[2];//here too so that it can be later painted, this is for debugging purpose
                points[2] = temp;
            }
        }
        for (int i = 0; i < points.Count; i++)//join left to right
        {
            InsertEdge(points[i], points[(i + 1) % points.Count]);
            //Debug.DrawLine(points[i].GetValue(), points[(i + 1) % points.Count].GetValue(), Color.blue, 99999999.9f);//Debugging purpouse to see the proccess
        }
    }

    public void InsertEdge(Node<Vector3> N1, Node<Vector3> N2)
    {
        AddAdjacency(N1, N2);
        AddAdjacency(N2, N1);

        if (!edges.Contains(new Tuple<Node<Vector3>, Node<Vector3>>(N1, N2)) && !edges.Contains(new Tuple<Node<Vector3>, Node<Vector3>>(N2, N1))) edges.Add(new Tuple<Node<Vector3>, Node<Vector3>>(N1, N2));
    }

    public void DeleteEdge(Node<Vector3> N1, Node<Vector3> N2)
    {
        edges.Remove(new Tuple<Node<Vector3>, Node<Vector3>>(N1, N2));
        edges.Remove(new Tuple<Node<Vector3>, Node<Vector3>>(N2, N1));

        N1.GetAdjacency().Remove(N2);
        N2.GetAdjacency().Remove(N1);
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
