using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    //Values to calculate Perlin Noise
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;
    [SerializeField] private float jump;

    //the values needed to determine the size of the plane and how many vertices and triangles it has
    bool isInitialized = false;//flag so that OnValidate doesnt act till everything is built, i dont really like this implemntation but i cant see right now an alternative
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 definition;//this definition value established how many vetices and triangls are created, will be used later

    [SerializeField]
    public Vector2 sizeProp//when changing the size redo the plane
    {
        get { return size; }
        set
        {
            size = value;

            CreatePlane();
        }
    }

    [SerializeField]
    public Vector2 definitionProp//when changing the definition redo the plane
    {
        get { return definition; }
        set
        {
            definition = value;

            CreatePlane();
        }
    }

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

        CreatePlane();

        isInitialized = true;
    }

    private void Start()
    {

    }

    private void CreatePlane()
    {
        vertices = PlaneGenerator.CreateVertices(definition, size);//extra step for better visualitazion on inspector
        triangles = PlaneGenerator.CreateTriangles(definition);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    // Update is called once per frame
    void Update()
    {
        PerlinFunction.DDraw(mesh, definition, vertices.ToArray(), speed, amplitude, frequency, jump);//dont even try to type anywhere meshFilter.mesh or everything fucking dies
    }

    //this triggers when a value is changed on th inspector
    private void OnValidate()
    {
        if (isInitialized)
        {
            definitionProp = definition;
            sizeProp = size;
        }
    }
}
