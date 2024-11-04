using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class QuickSort<T>//QuickSort algorithm
{
    public static void ASort(IComparer<T> c, LinkedList<T> L, int beginning, int ending)
    {
        if (ending <= beginning) return;

        int pivot = Recursion(c, L, beginning, ending, (a, b) => c.Compare(a, b) < 0);
        ASort(c, L, beginning, pivot - 1);//left recursion
        ASort(c, L, pivot + 1, ending);//right recursion
    }

    public static void DSort(IComparer<T> c, LinkedList<T> L, int beginning, int ending)
    {
        if (ending <= beginning) return;

        int pivot = Recursion(c, L, beginning, ending, (a, b) => c.Compare(a, b) > 0);
        DSort(c, L, beginning, pivot - 1);//left recursion
        DSort(c, L, pivot + 1, ending);//right recursion
    }

    static int Recursion(IComparer<T> c, LinkedList<T> L, int beginning, int ending, Func<T, T, bool> compare)//Sorting part
    {
        int i = beginning - 1;
        int pivot = ending;

        for (int j = beginning; j < ending; j++)
        {

            if (compare(L[j], L[pivot]))
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