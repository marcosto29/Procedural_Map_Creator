using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    //[SerializeField] private LineRenderer Line;
    //[SerializeField] private int points;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;
    [SerializeField] private float jump;
    [SerializeField] private Vector2 Limits;


    //the values needed to determine the size of the plane and how many vertices and triangles it has
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 definition;//this definition value established how many vetices and triangls are created, will be used later


    //the components reached to create the plane
    Mesh mesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;

    //the vertices and trianlges used to build the plane, use fo debugging purpose
    [SerializeField] List<Vector3> vertices;
    [SerializeField] List<int> triangles;
    

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        vertices = PlaneGenerator.CreateVertices(definition, size);//extra step for better visualitazion on inspector
        triangles = PlaneGenerator.CreateTriangles(definition);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PerlinFunction.DDraw(meshFilter, definition, vertices.ToArray(), speed, amplitude, frequency, jump);
    }
}
