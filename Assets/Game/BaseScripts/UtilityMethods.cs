using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityMethods
{
    private static System.Random random = new System.Random();

    public static int GenerateRandomIndex(int maxCount)
    {
        int randomIndex = random.Next(maxCount);
        return randomIndex;
    }

    public static IList<T> ShuffleList<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }    
}
