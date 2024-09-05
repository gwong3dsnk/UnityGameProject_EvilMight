using System;
using System.IO;
using UnityEngine;

public class Logger
{
    private static string logFilePath = Path.Combine(Application.dataPath, "Editor/Logs/Log.txt");

    public static void Log(string message)
    {
        try
        {
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(logFilePath);
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logMessage = $"{timeStamp} - {message}";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Write to the file
            File.AppendAllText(logFilePath, logMessage + "\n");
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to write log: " + ex.Message);
        }
    }
}
