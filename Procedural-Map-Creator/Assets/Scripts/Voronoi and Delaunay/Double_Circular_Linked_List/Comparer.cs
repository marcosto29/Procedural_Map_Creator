using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComparerV : IComparer<float>
{
    public int CompareZ(Vector3 X, Vector3 Y)
    {
        // First compare Z-coordinates
        if (X.z > Y.z) return 1;
        if (X.z < Y.z) return -1;

        // If z-coordinates are the same, compare y-coordinates
        if (X.x > Y.x) return 1;
        if (X.x < Y.x) return -1;

        return 0; // Points are equal
    }
    public int CompareX(Vector3 X, Vector3 Y)
    {
        // First compare x-coordinates
        if (X.x > Y.x) return 1;
        if (X.x < Y.x) return -1;

        // If x-coordinates are the same, compare z-coordinates
        if (X.z > Y.z) return 1;
        if (X.z < Y.z) return -1;

        return 0; // Points are equal
    }

    public int Compare(float M1, float M2)//order from lowest to highest on the right side of a line so that is counter - clockwise
    {
        if (M1 < M2) return -1;
        if (M1 > M2) return 1;
        return 0;
    }
}
