using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node <T>
{
    T value;
    readonly List<T> AdjacencyList;

    public Node(T v)
    {
        value = v;
        AdjancecyList = new();
    }

    public Node()
    {
        value = default;
        AdjancecyList = new();
    }
    
    public void SetFather(Node<T> n)
    {
        AdjacencyList[0] = n 
    }

    public Node<T> GetFather()
    {
        return AdjacencyList[0];
    }

    public void SetSon(Node<T> n)
    {
        AdjacencyList[AdjacencyList.count] = n;
    }

    public Node<T> GetSon()
    {
        return AdjacencyList[AdjacencyList.count];
    }

    public List<T> GetAdjancency()
    {
        return AdjancecyList;
    }

    public T GetValue()
    {
        return value;
    }

    public void SetValue(T v)
    {
        value = v;
    }
}
