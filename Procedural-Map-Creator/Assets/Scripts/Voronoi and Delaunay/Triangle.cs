using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Triangle
{
    public Node<Vector3>[] vertices;
    public Tuple<Node<Vector3>, Node<Vector3>>[] edges;

    public Triangle(Node<Vector3> A, Node<Vector3> B, Node<Vector3> C)
    {
        Vector3 aux1 = B.GetValue() - A.GetValue();
        Vector3 aux2 = C.GetValue() - A.GetValue();
        float crossProductZ = aux1.x * aux2.z - aux1.z * aux2.x;
        if (crossProductZ >= 0)//if they are clockwise swap them
        {
            Node<Vector3> temp = B;
            B = C;
            C = temp;
        }
        vertices = new Node<Vector3>[3] { A, B, C };
        edges = new Tuple<Node<Vector3>, Node<Vector3>>[3] { new Tuple<Node<Vector3>, Node<Vector3>>(A, B), new Tuple<Node<Vector3>, Node<Vector3>>(B, C), new Tuple<Node<Vector3>, Node<Vector3>>(C, A), };
    }

    public Triangle() { }
}
