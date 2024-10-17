using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TMG_EditorTools
{
    public class ScriptContentSearchTool : EditorWindow
    {
        private string folderPath = "Assets"; // The folder path to search.
        private string searchString = ""; // The string to check for.
        private bool caseSensitive = false; // Toggle for case sensitivity
        private bool recursiveSearch = true; // Toggle for recursive searching
        private Vector2 scrollPosition;
        private List<string> searchResults = new List<string>();

        [MenuItem("Tools/TMG_EditorTools/Script Content Search Tool")]
        public static void ShowWindow()
        {
            GetWindow<ScriptContentSearchTool>("Script Search Tool");
        }

        private void OnGUI()
        {
            GUILayout.Label("Script Content Search Tool", EditorStyles.boldLabel);

            folderPath = EditorGUILayout.TextField("Folder Path", folderPath);
            searchString = EditorGUILayout.TextField("Search String", searchString);
            caseSensitive = EditorGUILayout.Toggle("Case Sensitive", caseSensitive);
            recursiveSearch = EditorGUILayout.Toggle("Recursive Search", recursiveSearch);

            if (GUILayout.Button("Search"))
            {
                SearchInFolder(folderPath);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Search Results:");

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // Display search results
            for (int i = 0; i < searchResults.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(searchResults[i]);
                if (GUILayout.Button("Find", GUILayout.Width(50)))
                {
                    string filePath = GetFilePathFromResult(searchResults[i]);
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath));
                    }
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private string GetFilePathFromResult(string result)
        {
            int startIndex = result.IndexOf("Assets");
            if (startIndex != -1)
            {
                return result.Substring(startIndex);
            }
            return null;
        }


        private void SearchInFolder(string currentFolder)
        {
            searchResults.Clear(); // Clear previous search results

            StringComparison comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            SearchOption searchOption = recursiveSearch ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            if (Directory.Exists(currentFolder))
            {
                string[] files = Directory.GetFiles(currentFolder, "*.cs", searchOption);

                foreach (string file in files)
                {
                    string fileContent = File.ReadAllText(file);
                    if (fileContent.IndexOf(searchString, comparisonType) >= 0)
                    {
                        string result = "Found matching content in file: " + file;
                        searchResults.Add(result);
                        Debug.Log(result);
                    }
                }
            }
            else
            {
                string result = "Folder does not exist: " + currentFolder;
                searchResults.Add(result);
                Debug.Log(result);
            }
        }
    }
}