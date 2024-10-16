using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    //the components reached to create the plane
    [SerializeField] Mesh mesh;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    //the values needed to determine the size of the plane and how many vertices and triangles it has
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 definition;//this definition value established how many vetices and triangls are created, will be used later

    //the vertices and trianlges used to build the plane, use fo debugging purpose
    [SerializeField] List<Vector3> vertices;
    [SerializeField] List<int> triangles;

    [SerializeField] PerlinFunction pFunc;

    private void Awake()
    {
        pFunc = GetComponent<PerlinFunction>();
        mesh = new Mesh();//Create a new Mesh      
        meshFilter = GetComponent<MeshFilter>();//get the filter of the object to assigned it
        meshCollider = GetComponent<MeshCollider>();//get the collider of the object to assigned it
        meshCollider.sharedMesh = mesh;
        meshFilter.mesh = mesh;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        vertices = CreateVertices();//for debugging purpose
        triangles = CreateTriangles();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        meshFilter.mesh = mesh;

        pFunc.DDraw(meshFilter, definition);
    }

    List<Vector3> CreateVertices()
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

    List<int> CreateTriangles()
    {
        List<int> trian = new List<int>();

        for(int i = 0; i < definition.y; i++)
        {
            for(int j = 0; j < definition.x; j++)
            {
                int index = i * ((int)definition.x + 1) + j;//index position, for some reason (size.x + 1) has to be between parenthesis or it literally fucking explodes, im stupid 2 hours later i just learned operation order

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
