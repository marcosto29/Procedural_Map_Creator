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
}
