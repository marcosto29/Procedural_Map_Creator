using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node <T>
{
    T value;
    Node<T> child;
    Node<T> father;
    readonly List<T> AdjancecyList;

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
        father = n;
    }

    public Node<T> GetFather()
    {
        return father;
    }

    public void SetSon(Node<T> n)
    {
        child = n;
    }

    public Node<T> GetSon()
    {
        return child;
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
