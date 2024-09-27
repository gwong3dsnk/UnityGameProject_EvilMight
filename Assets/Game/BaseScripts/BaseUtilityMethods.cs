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
}
