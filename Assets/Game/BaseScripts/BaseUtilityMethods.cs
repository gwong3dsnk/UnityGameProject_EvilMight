using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class BaseUtilityMethods
{
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

    public static Vector3 GenerateRandomSpawnLocation(Vector3 spawnOrigin)
    {
        Vector2 random2DDirection = Random.insideUnitCircle.normalized;
        Vector3 direction3D = new Vector3(random2DDirection.x, 0, random2DDirection.y);
        float randomDistance = Random.Range(10.0f, 30.0f);
        return spawnOrigin + direction3D * randomDistance;
    }         
}
