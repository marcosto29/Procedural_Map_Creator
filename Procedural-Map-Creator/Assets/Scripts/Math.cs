using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Math
{
    public static float Lerp(float first, float second, float percentage)//calculate the number between two values given an X percentage
    {
        float totalDistance = second - first;//calculate the distance between the two values
        return first + (percentage * totalDistance);//multiply the percentage required to the totalDistance distance and add it to the first value to get the wanted point
    }

    public static float PerlinNoise(float x, float y, float timerCount, float speed, float frequency, float amplitude)
    {
        //unlike Random function when calculating a Perlin Value it does it through a number time variable (the value is fixed on that seed) and not between the range of 2 number variables
        return Mathf.PerlinNoise((x + timerCount * speed) * frequency, (y + timerCount * speed) * frequency) * amplitude;//The variable timerCount makes it so that the graph changes over time and the variable speed how fast does it
    }

    public static List<Vector3> Bisector(Vector3 A, Vector3 B, Vector2 boundaries) {
        //formula to calculate vectors between 2 points = 1/2(u + v) being u OA and v 0B, this vectors are equal to the respectives points since they start from [0,0]
        Vector3 midPoint = MidPoint(A, B);
        Vector3 vectorAB = B - A;
        Vector3 perpendicularVector = new Vector3(-vectorAB.z, 0, vectorAB.x);
        //mathematically the equation of a line that pass through a point and with a vector is = point + t(vector)
        // x = point.x + vector.x * t
        // z = point.z + vector.z * t
        // z = point.z + vector.z * ((x - point.x)/vector.x) this is the equation of the line and we now that it has to go at least through 4 boundaries
        // x = point.x + vector.x * ((z - point.z)/vector.z)
        
        
        List<Vector3> points = new ();
     
        float x = midPoint.x + perpendicularVector.x * ((boundaries.y - midPoint.z) / perpendicularVector.z);//this thing needs to be optimize but basically is geometry math
        float z = midPoint.z + perpendicularVector.z * ((boundaries.x - midPoint.x) / perpendicularVector.x);
        float x2 = midPoint.x + perpendicularVector.x * ((0 - midPoint.z) / perpendicularVector.z);
        float z2 = midPoint.z + perpendicularVector.z * ((0 - midPoint.x) / perpendicularVector.x);
        if (x <= boundaries.x && x >= 0) points.Add(new Vector3(x, 0, boundaries.y));
        if (z <= boundaries.y && z >= 0) points.Add(new Vector3(boundaries.x, 0, z));
        if (x2 >= 0 && x2 <= boundaries.x) points.Add(new Vector3(x2, 0, 0));
        if (z2 >= 0 && z2 <= boundaries.y) points.Add(new Vector3(0, 0, z2));

        return points;
    }

    public static Vector3 MidPoint(Vector3 P1, Vector3 P2)
    {
        Vector3 auxVector = P1 + P2;
        return new Vector3(auxVector.x / 2, auxVector.y / 2, auxVector.z / 2);
    }

    public static bool CheckColinear(LinkedList<Vector3> vertices)
    {
        //Ax * (By - Cy) + Bx * (Cy - Ay) + Cx * (Ay - By) / 2 check if the area of the triangle is 0, doesnt need to divide by 2
        if (vertices.count >= 3) return (vertices[0].x * (vertices[1].z - vertices[2].z) + vertices[1].x * (vertices[2].z - vertices[0].z) + vertices[2].x * (vertices[0].z - vertices[1].z)) == 0;
        //when handling only 2 points catch the exception and send a true since a line will be created
        return true;
    }

    public static float Distance(Vector3 V, Vector3 V2, Vector3 Y)
    {
        return Vector3.Magnitude(Vector3.Cross(V2 - V, V - Y)) / Vector3.Magnitude(V2 - V);
    }

    public static bool IsRight(Vector3 A, Vector3 B, Vector3 M)
    {
        //cross product of AM and AB
        //AM = M - A
        //AB = B - A
        Vector3 AM = M - A;
        Vector3 AB = B - A;
        Vector3 Orthogonal = Vector3.Cross(AB, AM);
        return (Orthogonal.y > 0);
    }

    public static bool IsLeft(Vector3 A, Vector3 B, Vector3 M)
    {
        //cross product of AM and AB
        //AM = M - A
        //AB = B - A
        Vector3 AM = M - A;
        Vector3 AB = B - A;
        Vector3 Orthogonal = Vector3.Cross(AB, AM);
        return (Orthogonal.y < 0);
    }

    public static Vector3 ChosenPoint(LinkedList<Vector3> auxV, Func<Vector3, Vector3, bool> comparer)
    {
        QuickSort<Vector3>.Sort(auxV, 0, auxV.count - 1, comparer);//Sorting the List
        return auxV[0];
    }

    public static bool QTest(Vector3 P1 ,Vector3 P2, Vector3 P3, Vector3 Q)//Test if the point Q is inside the circumcircle of P1, P2, P3
    {
        Vector3 midPoint1 = MidPoint(P1, P2);
        float pSlope = -1 / ((P2.z - P1.z) / (P2.x - P1.x)); 

        Vector3 midPoint2 = MidPoint(P2, P3);
        float pSlope2 = -1 / ((P3.z - P2.z) / (P3.x - P2.x));

        //formula y - midPoint.z = perpendicularSlope * (x - midPoint.x)

        float x = (- pSlope2 * midPoint2.x + midPoint2.z + pSlope * midPoint1.x - midPoint1.z) / (pSlope - pSlope2);
        float y = pSlope * (x - midPoint1.x) + midPoint1.z;

        Vector3 center = new(x, 0, y);

        float epsilon = 1e-4f; ;

        float radius = (center - P1).sqrMagnitude;
        float distance2 = (center - Q).sqrMagnitude;

        if (distance2 < radius - epsilon) return false;
        return true;
    }


}
