using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node <T>
{
    T value;
    LinkedList<Node<T>> AdjacencyList;

    public Node(T v)
    {
        value = v;
        AdjacencyList = new();
    }

    public Node()
    {
        value = default;
        AdjacencyList = new();
    }

    public void SetFather(Node<T> n)
    {
        AdjacencyList.AddFirst(n);
    }

    public Node<T> GetFather()
    {
        return AdjacencyList.First.Value;
    }

    public void SetSon(Node<T> n)
    {
        AdjacencyList.AddLast(n);
    }

    public Node<T> GetSon()
    {
        return AdjacencyList.Last.Value;
    }

    public T GetValue()
    {
        return value;
    }

    public void SetValue(T v)
    {
        value = v;
    }

    public LinkedList<Node<T>> GetAdjacency()
    {
        return AdjacencyList;
    }
}
