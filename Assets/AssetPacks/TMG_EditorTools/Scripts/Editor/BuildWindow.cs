using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace TMG_EditorTools
{
    
public class BuildWindow : EditorWindow
{
    private string buildPath = "C:/Finished Builds";
    private bool isDemoBuild = false;

    // File paths for storing scene paths, game names, and app ids
    private string demoLevelsFilePath = "Assets/demoLevels.txt";
    private string retailLevelsFilePath = "Assets/retailLevels.txt";
    private string gameNamesFilePath = "Assets/gameNames.txt"; 
    private string appIdsFilePath = "Assets/appIds.txt";  // New file for storing app ids

    // Level paths for Demo and Retail
    private string[] demoLevels = new string[]
    {
        "Assets/Game.unity",
    };

    private string[] retailLevels = new string[]
    {
        "Assets/Game.unity",
    };

    // Game name, demo name, and appId fields
    private string gameName = "GameName"; // Default game name
    private string retailAppId = "480"; // Default app id for retail
    private string demoName = "GameName_DEMO"; // Default demo name
    private string demoAppId = "480";  // Default app id for demo

    // Scroll position for the scroll view
    private Vector2 scrollPosition;

    [MenuItem("Tools/TMG_EditorTools/Steam Build Window")]
    public static void ShowWindow()
    {
        GetWindow<BuildWindow>("Final Steam Build");
    }

    private void OnEnable()
    {
        LoadScenePaths();
        LoadGameNames(); 
        LoadAppIds();  // Load app ids on enable
    }

    private void OnDisable()
    {
        SaveScenePaths();
        SaveGameNames();   
        SaveAppIds();  // Save app ids on close
    }

    private void OnGUI()
    {
        GUILayout.Label("Final Build Options", EditorStyles.boldLabel);

        isDemoBuild = GUILayout.Toggle(isDemoBuild, "Demo Build");
        GUILayout.Space(5);
        GUILayout.Label("Build Location:", EditorStyles.label);

        GUILayout.BeginHorizontal();
        GUILayout.TextField(buildPath);
        if (GUILayout.Button("Select Folder"))
        {
            buildPath = EditorUtility.SaveFolderPanel("Choose Location of Built Game", buildPath, "");
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        // Input fields for game name and demo name
        GUILayout.Label("Retail Name:", EditorStyles.label);
        gameName = GUILayout.TextField(gameName);

        GUILayout.Label("Retail App ID:", EditorStyles.label);
        retailAppId = GUILayout.TextField(retailAppId);
        
        GUILayout.Label("Demo Name:", EditorStyles.label);
        demoName = GUILayout.TextField(demoName);

        // Input fields for app ids
        GUILayout.Label("Demo App ID:", EditorStyles.label);
        demoAppId = GUILayout.TextField(demoAppId);


        GUILayout.Space(10);

        if (GUILayout.Button("Build Game"))
        {
            SaveScenePaths();
            SaveGameNames();   
            SaveAppIds();  

            if (isDemoBuild)
            {
                BuildGameDemo();
            }
            else
            {
                BuildGameRetail();
            }
        }

        GUILayout.Label("Scene Paths:", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        if (isDemoBuild)
        {
            DisplayLevelPaths(ref demoLevels, "Demo");
        }
        else
        {
            DisplayLevelPaths(ref retailLevels, "Retail");
        }

        GUILayout.EndScrollView();
        GUILayout.Space(2);
    }

    private void DisplayLevelPaths(ref string[] levelPaths, string buildType)
    {
        for (int i = 0; i < levelPaths.Length; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(i.ToString(), GUILayout.Width(20));
            levelPaths[i] = GUILayout.TextField(levelPaths[i]);
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button($"Add {buildType} Level"))
        {
            Array.Resize(ref levelPaths, levelPaths.Length + 1);
            levelPaths[levelPaths.Length - 1] = "New Level Path";
        }

        if (levelPaths.Length > 1)
        {
            if (GUILayout.Button($"Remove Last {buildType} Level"))
            {
                Array.Resize(ref levelPaths, levelPaths.Length - 1);
            }
        }
    }

    private void BuildGameDemo()
    {
        WriteStringDemo();
        BuildPipeline.BuildPlayer(demoLevels, Path.Combine(buildPath, $"{demoName}.exe"), BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    private void BuildGameRetail()
    {
        WriteStringRetail();
        BuildPipeline.BuildPlayer(retailLevels, Path.Combine(buildPath, $"{gameName}.exe"), BuildTarget.StandaloneWindows, BuildOptions.None);
    }

    private void WriteStringDemo()
    {
        string filePath = "steam_appid.txt";
        File.Delete(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(demoAppId);  // Write demo app id
        }
    }

    private void WriteStringRetail()
    {
        string filePath = "steam_appid.txt";
        File.Delete(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(retailAppId);  // Write retail app id
        }
    }

    private void SaveScenePaths()
    {
        File.WriteAllLines(demoLevelsFilePath, demoLevels);
        File.WriteAllLines(retailLevelsFilePath, retailLevels);
    }

    private void LoadScenePaths()
    {
        if (File.Exists(demoLevelsFilePath))
        {
            demoLevels = File.ReadAllLines(demoLevelsFilePath);
        }

        if (File.Exists(retailLevelsFilePath))
        {
            retailLevels = File.ReadAllLines(retailLevelsFilePath);
        }
    }

    private void SaveGameNames()
    {
        using (StreamWriter writer = new StreamWriter(gameNamesFilePath))
        {
            writer.WriteLine(gameName);
            writer.WriteLine(demoName);
        }
    }

    private void LoadGameNames()
    {
        if (File.Exists(gameNamesFilePath))
        {
            string[] lines = File.ReadAllLines(gameNamesFilePath);
            if (lines.Length >= 2)
            {
                gameName = lines[0];
                demoName = lines[1];
            }
        }
    }

    private void SaveAppIds()
    {
        using (StreamWriter writer = new StreamWriter(appIdsFilePath))
        {
            writer.WriteLine(retailAppId);
            writer.WriteLine(demoAppId);
        }
    }

    private void LoadAppIds()
    {
        if (File.Exists(appIdsFilePath))
        {
            string[] lines = File.ReadAllLines(appIdsFilePath);
            if (lines.Length >= 2)
            {
                retailAppId = lines[0];
                demoAppId = lines[1];
            }
        }
    }
}
}

