using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuickSort<T>//QuickSort algorithm
{
    public static void AscendingSort(IComparer<T> c, LinkedList<T> L, int beginning, int ending)
    {
        if (ending <= beginning) return;

        int pivot = Recursion(c, L, beginning, ending);
        AscendingSort(c, L, beginning, pivot - 1);//left recursion
        AscendingSort(c, L, pivot + 1, ending);//right recursion
    }

    static int Recursion(IComparer<T> c, LinkedList<T> L, int beginning, int ending)
    {
        int i = beginning - 1;
        int pivot = ending;

        for (int j = beginning; j < ending; j++)
        {
            if (c.Compare(L[j].GetValue(), L[pivot].GetValue()) < 0)
            {
                i++;
                SwapNodes(L, L[i], L[j]);
            }
        }
        i++;
        SwapNodes(L, L[i], L[pivot]);
        return i;
    }

    static void SwapNodes(LinkedList<T> L, Node<T> a, Node<T> b)
    {
        T temp = a.value;
        a.value = b.value;
        b.value = temp;
    }
}

