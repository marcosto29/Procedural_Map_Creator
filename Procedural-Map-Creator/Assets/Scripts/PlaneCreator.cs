using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlaneCreator : MonoBehaviour
{
    //Values to calculate Perlin Noise
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;

    Mesh mesh;
    MeshCollider meshCollider;
    MeshFilter meshFilter;

    [SerializeField] Vector2 size;
    [SerializeField] int points;

    void Awake()
    {
        Tuple<Vector3[], int[]> safe = DelaunayTriangulation.Delaunay(size, points);

        mesh = new();

        mesh.vertices = safe.Item1;
        mesh.triangles = safe.Item2;

        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }
    private void Update()
    {
        PerlinFunction.DDraw(mesh, size, mesh.vertices, speed, amplitude, frequency);
        mesh.RecalculateBounds();
    }
}
