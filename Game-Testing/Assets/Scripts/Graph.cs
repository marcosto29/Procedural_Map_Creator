using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] private List<float> points;
    private GameObject NoisePoint;
    private GameObject Interpolation;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Aux = new GameObject();
        points = PerlinNoise.GenerateNoise();
        for(int i = 0; i < points.Count; i++)
        {
            float z = (i % this.points.Count - (this.points.Count/2));
            NoisePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            NoisePoint.transform.position = new Vector3(this.transform.position.x, points[i], z);

            if (i >= 1)
            {
                Interpolation = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

                Vector3 direction = Aux.transform.position - NoisePoint.transform.position;
                float size = direction.magnitude;

                Interpolation.transform.position = (NoisePoint.transform.position + Aux.transform.position) / 2F;
                Interpolation.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
                Interpolation.transform.localScale = new Vector3(Interpolation.transform.localScale.x, size / 2f, Interpolation.transform.localScale.z);
            }
            Aux = NoisePoint;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
