using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Vector3 auxVector = A + B;
        Vector3 midPoint = new Vector3(auxVector.x/2, auxVector.y/2, auxVector.z/2);
        Vector3 vectorAB = B - A;
        Vector3 perpendicularVector = new Vector3(-vectorAB.z, 0, vectorAB.x);
        //mathematically the equation of a line that pass through a point and with a vector is = point + t(vector)
        // x = point.x + vector.x * t
        // z = point.z + vector.z * t
        // z = point.z + vector.z * ((x - point.x)/vector.x) this is the equation of the line and we now that it has to go at least through 4 boundaries
        // x = point.x + vector.x * ((z - point.z)/vector.z)
        List<Vector3> points = new List<Vector3>();



        //THIS FOR THE LOVE OF GOD NEEDS TO BE CHANGED TO SOMETHING  BETTER OH GOD EVERY TIME I SEE IT I WANT TO KEEP MYSELF SAFE (KMS)       
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
    public static bool CheckColinear(LinkedList<Vector3> vertices)
    {
        //Ax * (By - Cy) + Bx * (Cy - Ay) + Cx * (Ay - By) / 2 check if the area of the triangle is 0, doesnt need to divide by 2
        if (vertices.count >= 3) return (vertices[0].x * (vertices[1].z - vertices[2].z) + vertices[1].x * (vertices[2].z - vertices[0].z) + vertices[2].x * (vertices[0].z - vertices[1].z)) == 0;
        //when handling only 2 points catch the exception and send a true since a line will be created
        return true;
    }

}
