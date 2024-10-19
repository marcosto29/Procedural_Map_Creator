using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField] private LineRenderer Line;
    [SerializeField] private int points;
    [SerializeField] private Vector2 Limits;

    //Values to calculate Perlin Noise
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;
    [SerializeField] private float speed;
    [SerializeField] private float jump;

    private void Awake()
    {
        Line = GetComponent<LineRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PerlinFunction.Draw(Limits, Line, points, jump, speed, amplitude, frequency);
    }
}
