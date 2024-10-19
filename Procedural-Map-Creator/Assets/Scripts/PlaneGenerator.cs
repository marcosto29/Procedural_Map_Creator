using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlaneGenerator
{
    public static List<Vector3> CreateVertices(Vector2 definition, Vector2 size)//this function gotta be remade to try and approch it with the voroni diagram/delaunay triangulation
    {
        List<Vector3> vert = new List<Vector3>();
        for(int i = 0; i <= definition.y; i++)
        {
            for(int j = 0; j <= definition.x; j++)
            {
                vert.Add(new Vector3(size.x*(j/definition.x), 0, size.y*(i/definition.y)));
            }
        }
        return vert;
    }

    public static List<int> CreateTriangles(Vector2 definition)
    {
        List<int> trian = new List<int>();

        for(int i = 0; i < definition.y; i++)
        {
            for(int j = 0; j < definition.x; j++)
            {
                int index = i * ((int)definition.x + 1) + j;//index position

                // First triangle (top-left, bottom-left, bottom-right) in thise order (counter-clockwise) the plane renders inside out
                trian.Add(index);
                trian.Add(index + (int)definition.x + 1);
                trian.Add(index + (int)definition.x + 2);

                // Second triangle (top-left, bottom-right, top-right)
                trian.Add(index);
                trian.Add(index + (int)definition.x + 2);
                trian.Add(index + 1);


                // First triangle (top-left, top-right, bottom-right) in this order (clockwise) the plane renders right
                trian.Add(index);
                trian.Add(index + 1);
                trian.Add(index + (int)definition.x + 2);

                // Second triangle (top-left, bottom-right, bottom-left)
                trian.Add(index);
                trian.Add(index + (int)definition.x + 2);
                trian.Add(index + (int)definition.x + 1);

                //Doing both clock directions, a 2 side plane can be render
            }
        }
        return trian;
    }
}
