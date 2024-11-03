using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node <T>
{
    public T value;
    Node<T> child;
    Node<T> father;

    public Node(T v)
    {
        value = v;
    }

    public Node()
    {
        value = default(T);
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

    public T GetValue()
    {
        return value;
    }

    public void SetValue(T v)
    {
        value = v;
    }
}
