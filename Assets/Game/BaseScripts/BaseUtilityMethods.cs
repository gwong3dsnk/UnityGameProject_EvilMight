using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class BaseUtilityMethods
{
    private static System.Random random = new System.Random();

    public static int GenerateRandomIndex(int maxCount)
    {
        int randomIndex = random.Next(maxCount);
        return randomIndex;
    }

    public static List<T> ShuffleList<T>(List<T> list)
    {
        // Fisher-Yates Shuffle algorithm to mix up a provided list.
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

    public static string InsertSpaceBeforeCapitalLetters(string input)
    {
        // Use regex to insert a space before each capital letter, except the first one
        return Regex.Replace(input, @"(?<!^)(?<!^)([A-Z])", " $1");
    }    

    public static void DoesDirectoryExist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }        
    }
}
