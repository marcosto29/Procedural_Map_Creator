using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComparerV : IComparer<Vector3>
{
    public int Compare(Vector3 X, Vector3 Y)
    {
        // First compare x-coordinates
        if (X.x > Y.x) return 1;
        if (X.x < Y.x) return -1;

        // If x-coordinates are the same, compare y-coordinates
        if (X.y > Y.y) return 1;
        if (X.y < Y.y) return -1;

        return 0; // Points are equal
    }

    public int Compare(float C1, float C2, float M1, float M2)//order from lowest to highest on the right side of a line so that is counter - clockwise
    {
        if (C1 < 0 && C2 < 0) 
        {
            if (M1 < M2) return 1;   
            else if (M1 == M2) return 0; 
            else return -1;            
        }
        else if (C1 >= 0 && C2 >= 0) 
        {
            if (M1 < M2) return 1;  
            else if (M1 == M2) return 0;
            else return -1;
        }
        else if (C1 < 0 && C2 >= 0) return 1;
        return -1;
    }
}
