using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class QuickSort<T>//QuickSort algorithm
{
    public static void Sort(List<T> L, int beginning, int ending, Func<T, T, bool> compare)
    {
        if (ending <= beginning) return;

        int pivot = Recursion(L, beginning, ending, compare);
        Sort(L, beginning, pivot - 1, compare);//left recursion
        Sort(L, pivot + 1, ending, compare);//right recursion
    }

    static int Recursion(List<T> L, int beginning, int ending, Func<T, T, bool> compare)//Sorting part
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
    static void SwapNodes(List<T> L, int a, int b)
    {
        T temp = L[a];
        L[a] = L[b];
        L[b] = temp;
    }
}