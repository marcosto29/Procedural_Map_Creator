using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hull
{
    public LinkedList<Tuple<Vector3, Vector3>> edges;
    public LinkedList<Vector3> points;
    public LinkedList<Vector3> edgePoints;

    public Hull()
    {
        edges = new();
        points = new();
        edgePoints = new();
    }
    public Hull(int start, int end, LinkedList<Vector3> vertices)
    {
        edges = new();
        points = new();
        edgePoints = new();

        for (int i = start; i < end; i++) points.Add(vertices[i]);

        //check wether the vertices are colinear to instead of drawing a triangle just join them
        if (!Math.CheckColinear(points))
        {
            Vector3 aux1 = points[1] - points[0];
            Vector3 aux2 = points[2] - points[0];
            float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
            if (crossProductZ <= 0)//if they are clockwise swap them
            {
                Vector3 temp = points[1];
                vertices.Get(points[2]).SetValue(temp);
                vertices.Get(points[1]).SetValue(points[2]);//swap them on the array

                points[1] = points[2];//here too so that it can be later painted, this is for debugging purpose
                points[2] = temp;
            }
        }
        for (int i = 0; i < points.count; i++)//join left to right
        {
            edgePoints.Add(points[i]);
            Debug.DrawLine(points[i], points[(i + 1) % points.count], Color.blue, 99999999.9f);//Debugging purpouse to see the proccess
            edges.Add(new Tuple<Vector3, Vector3>(points[i], points[(i + 1) % points.count]));
        }
    }
    public Vector3 FollowingPoint(Vector3 V, Vector3 V2, string sequence)
    {
        if (edgePoints.count == 2) return (edgePoints[0] == V) ? edgePoints[1] : edgePoints[0];//segment case

        //create an aux list that will contain each point with the distance to the segment and whether is on the right or left side
        LinkedList<Tuple<Vector3, bool, float>> auxVectors = new();
        for (int i = 0; i < edgePoints.count; i++)
        {
            bool isRight = Math.IsRight(V, V2, edgePoints[i]);
            float angle = Vector3.Angle(edgePoints[i] - V, V2 - V);//calculate the angle and if the point is on the right or left side of the segmente

            if ((sequence == "Left" && isRight) || (sequence == "Right" && !isRight))// since Vector3.angle only gives a number between 0 and 180 the case where the angle is concave needs to be treated
            {
                angle = 360 - angle;
            }

            if (edgePoints[i] != V2 && edgePoints[i] != V) auxVectors.Add(new Tuple<Vector3, bool, float>(edgePoints[i], isRight, angle));//filter the point of the hull being checked
        }
        //sort them from closest to farthest
        QuickSort<Tuple<Vector3, bool, float>>.Sort(auxVectors, 0, auxVectors.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);//sort the points to know which one is the closest to the one being cheked 

        return auxVectors[0].Item1;
    }
}
