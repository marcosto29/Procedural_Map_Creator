using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedList<T>
{
    Node<T> father;
    int count;
    public LinkedList() {

        father = null;
        count = 0;
    }

    public T this[int i]
    {
        get
        {
            if (i < 0 || i >= count)
                throw new System.IndexOutOfRangeException("Index out of range");
            return GetNode(i).value;
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
        iterator.value = v;
    }

    public void AscendingSort(IComparer<T> c)
    {
        
        LinkedList<T> Sorted = new LinkedList<T>();
        int j = 0;
        Node<T> sIterator = Sorted.father;
        Node<T> nIterator = father;
        while (j < count)
        {
            while (sIterator != null && c.Compare(nIterator.GetValue(), sIterator.GetValue()) >= 0)
            {
                sIterator = sIterator.GetFather();
            }

            j++;
        }
    }
}
