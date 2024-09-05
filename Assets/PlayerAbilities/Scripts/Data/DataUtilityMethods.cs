using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class DataUtilityMethods
{
    public static List<string> GetAssetPrefabNamesByPath(string path)
    {
        List<string> prefabNames = new List<string>();
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { path });

        if (prefabGuids.Length > 0)
        {
            foreach (string guid in prefabGuids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                prefabNames.Add(prefab.name);
            }

            return prefabNames;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Takes in the list of ability prefab names and repopulates the AbilityNames enum file which will update the Ability and Upgrade
    /// ScriptableObject dropdown menus.
    /// </summary>
    /// <param name="stringText"></param>
    public static void WriteNewAbilityEnums(List<string> stringText, string enumFilePath)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("public enum AbilityNames");
        sb.AppendLine("{");
        
        for (int i = 0; i < stringText.Count; i++)
        {
            if (i == stringText.Count - 1)
            {
                sb.AppendLine($"{stringText[i]}");
            }
            else
            {
                sb.AppendLine($"{stringText[i]},");
            }
        }

        sb.AppendLine("}");

        File.WriteAllText(enumFilePath, sb.ToString());
    }    
}
