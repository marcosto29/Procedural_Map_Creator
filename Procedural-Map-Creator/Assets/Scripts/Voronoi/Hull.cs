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
        if (edgePoints.count == 2) return (edgePoints[0] == V2 || edgePoints[0] == V) ? edgePoints[1] : edgePoints[0];//segment case

        //create an aux list that will contain each point with the distance to the segment and whether is on the right or left side
        LinkedList<Tuple<Vector3, bool, float>> auxVectors = new();
        for (int i = 0; i < edgePoints.count; i++) auxVectors.Add(new Tuple<Vector3, bool, float>(edgePoints[i], Math.IsRight(V, V2, edgePoints[i]), Vector3.Angle(edgePoints[i] - V, V2 - V)));

        //sort them from closest to farthest
        QuickSort<Tuple<Vector3, bool, float>>.Sort(auxVectors, 0, auxVectors.count - 1, (a, b) => new ComparerV().Compare(a.Item3, b.Item3) < 0);

        //divide the list in two, the points on the right and the points on the left side
        LinkedList<Tuple<Vector3, bool, float>> leftPoints = new();
        LinkedList<Tuple<Vector3, bool, float>> rightPoints = new();

        //filter the points that are on the line
        for (int i = 0; i < auxVectors.count; i++)
        {
            if (!auxVectors[i].Item2 && auxVectors[i].Item3 != 0) leftPoints.Add(auxVectors[i]);
            else if (auxVectors[i].Item2 && auxVectors[i].Item3 != 0) rightPoints.Add(auxVectors[i]);
        }
  
        //return or the closest point on the left or right depending on what the code is asking
        if(sequence == "Pred")
        {
            for (int i = 0; i < leftPoints.count; i++) rightPoints.Add(leftPoints[i]);
            return rightPoints[0].Item1;
        }
        else
        {
            for (int i = 0; i < rightPoints.count; i++) leftPoints.Add(rightPoints[i]);
            return leftPoints[0].Item1;
        }
    }
}
