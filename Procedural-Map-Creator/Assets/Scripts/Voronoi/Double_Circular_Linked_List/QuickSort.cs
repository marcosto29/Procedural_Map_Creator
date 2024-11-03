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

    static int Recursion(IComparer<T> c, LinkedList<T> L, int beginning, int ending)//Sorting part
    {
        int i = beginning - 1;
        int pivot = ending;

        for (int j = beginning; j < ending; j++)
        {
            if (c.Compare(L[j], L[pivot]) < 0)
            {
                i++;
                SwapNodes(L, i, j);
            }
        }
        i++;
        SwapNodes(L, i, pivot);
        return i;
    }

    static void SwapNodes(LinkedList<T> L, int a, int b)
    {
        T temp = L[a];
        L[a] = L[b];
        L[b] = temp;
    }
}