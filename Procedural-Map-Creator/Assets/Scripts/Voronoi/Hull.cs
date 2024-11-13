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
    public Vector3 FollowingPoint(Vector3 V, Vector3 V2, string sequence)
    {
        if (edgePoints.count == 2) return (edgePoints[0] == V) ? edgePoints[1] : edgePoints[0];//segment case

        //create an aux list that will contain each point with the distance to the segment and whether is on the right or left side
        LinkedList<Tuple<Vector3, bool, float>> auxVectors = new();
        for (int i = 0; i < edgePoints.count; i++)
        {
            bool isRight = Math.IsRight(V, V2, edgePoints[i]);
            float angle = Vector3.Angle(edgePoints[i] - V, V2 - V);
            if(sequence == "Left")
            {
                if (isRight) angle = 360 - angle;
            }
            else
            {
                if (!isRight) angle = 360 - angle;
            }
            
            if(edgePoints[i] != V2 && edgePoints[i] != V) auxVectors.Add(new Tuple<Vector3, bool, float>(edgePoints[i], isRight, angle));
        }
        //sort them from closest to farthest
        QuickSort<Tuple<Vector3, bool, float>>.Sort(auxVectors, 0, auxVectors.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);

        return auxVectors[0].Item1;
    }
}
