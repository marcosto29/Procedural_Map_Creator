using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneManager : MonoBehaviour
{
    //Values to calculate Perlin Noise
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;

    //the values needed to determine the size of the plane and how many vertices and triangles it has
    bool isInitialized = false;//flag so that OnValidate doesnt act till everything is built, i dont really like this implemntation but i cant see right now an alternative
    [SerializeField] Vector2 size;
    [SerializeField] Vector2 definition;//this definition value established how many vetices and triangls are created, will be used later

    private Action planeCreate;

    //private delegate void Creator(); //delegate with more steps
    //private event Creator planeCreate;

    [SerializeField]
    public Vector2 SizeProp//when changing the size redo the plane
    {
        get { return size; }
        set
        {
            size = value;
            
            planeCreate();
        }
    }

    [SerializeField]
    public Vector2 DefinitionProp//when changing the definition redo the plane
    {
        get { return definition; }
        set
        {
            definition = new Vector2((int)value.x, (int)value.y);

            planeCreate();
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

        //planeCreate = CreatePlane;//delegate with more steps

        planeCreate += () => {//delegate with lambda

            mesh.Clear();//this is optional, but if the mesh doesnt get refresh things might get spooky

            Vector2 intDef = new Vector2((int)definition.x, (int)definition.y);

            vertices = BasicPlane.CreateVertices(intDef, size);//extra step for better visualitazion on inspector
            triangles = BasicPlane.CreateTriangles(intDef);

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();

            mesh.RecalculateBounds();//this should be calles too when changing the geometry of the plane, unity doesnt like when you dont do it
        };

        isInitialized = true;

        planeCreate();
    }

    // Update is called once per frame
    void Update()
    {
        PerlinFunction.DDraw(mesh, definition, vertices.ToArray(), speed, amplitude, frequency);//dont even try to type anywhere meshFilter.mesh or everything fucking dies
    }

    //this triggers when a value is changed on th inspector
    private void OnValidate()
    {
        if (isInitialized)
        {
            DefinitionProp = definition;
            SizeProp = size;
        }
    }

    //private void CreatePlane()//for if i need it instead of the delegate
    //{
    //    vertices = PlaneGenerator.CreateVertices(definition, size);//extra step for better visualitazion on inspector
    //    triangles = PlaneGenerator.CreateTriangles(definition);

    //    mesh.vertices = vertices.ToArray();
    //    mesh.triangles = triangles.ToArray();
    //}
}
