using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedList<T>
{
    public Node<T> father;
    public int count;
    public LinkedList() {

        father = null;
        count = 0;
    }

    public LinkedList(LinkedList<T> L)
    {
        if (L.father == null)
        {
            this.father = null;
            this.count = 0;
        }
        else
        {
            for(int i = 0; i < L.count; i++) this.Add(L[i]);
        }
    }

    public T this[int i]
    {
        get
        {
            if (i < 0 || i >= count)
                throw new System.IndexOutOfRangeException("Index out of range");
            return GetNode(i).GetValue();
        }
        set
        {
            if (i < 0 || i >= count)
                throw new System.IndexOutOfRangeException("Index out of range");
            SetNode(i, value);
        }
    }

    public void Add(T v)
    {
        Node<T> n = new Node<T>(v);
        if (father == null)
        {
            father = n;
            father.SetFather(father);
            father.SetSon(father);
        }
        else
        {
            Node<T> iterator = father;
            while (iterator.GetSon() != father)
            {
                iterator = iterator.GetSon();
            }
            n.SetFather(iterator);
            n.SetSon(father);
            iterator.SetSon(n);
            father.SetFather(n);
        }
        count++;
    }
    public void Remove(int i)
    {
        if (i < 0 || i >= count)
            throw new System.IndexOutOfRangeException("Index out of range");
        if (father != null)
        {
            int j = 0;
            Node<T> curr = father;
            while (j < i)
            {
                curr = curr.GetSon();
            }
            if (curr != father)
            {
                Node<T> aux = curr.GetFather();
                aux.SetSon(curr.GetSon());
                curr.GetSon().SetFather(aux);
            }
            else father = null;
            count--;
        }
    }

    private Node<T> GetNode(int i)
    {
        Node<T> iterator = father;
        int j = 0;
        while(j < i)
        {
            iterator = iterator.GetSon();
            j++;
        }
        return iterator;
    }

    private void SetNode(int i, T v)
    {
        Node<T> iterator = father;
        int j = 0;
        while (j < i)
        {
            iterator = iterator.GetSon();
            j++;
        }
        iterator.SetValue(v);
    }

    public void Print()
    {
        int j = 0;
        Node<T> iterator = father;
        while(j < count)
        {
            Debug.Log(iterator.GetValue());
            iterator = iterator.GetSon();
            j++;
        }
    }

    public Node<T> Get(T value)
    {
        int j = 0;
        Node<T> iterator = father;
        while (j < count)
        {
            if(EqualityComparer<T>.Default.Equals(iterator.GetValue(), value)) return iterator;
            iterator = iterator.GetSon();
            j++;
        }
        return null;
    }

    public bool isEmpty()
    {
        if (count <= 0) return true;
        return false;
    }

    public void Remove(T value)
    {
        int j = 0;
        Node<T> iterator = father;
        while (j < count)
        {
            if (EqualityComparer<T>.Default.Equals(iterator.GetValue(), value))
            {
                Node<T> aux = iterator.GetFather();
                aux.SetSon(iterator.GetSon());
                iterator.GetSon().SetFather(aux);
            }
            iterator = iterator.GetSon();
            j++;
            count--;
        }
    }
}
