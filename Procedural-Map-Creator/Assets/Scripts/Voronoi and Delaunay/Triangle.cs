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
        vertices = new Node<Vector3>[3] { A, B, C };
        edges = new Tuple<Node<Vector3>, Node<Vector3>>[3] { new Tuple<Node<Vector3>, Node<Vector3>>(A, B), new Tuple<Node<Vector3>, Node<Vector3>>(B, C), new Tuple<Node<Vector3>, Node<Vector3>>(C, A), };
    }

    public Triangle() { }
}
