using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LogFileManager : MonoBehaviour
{
    private readonly long MaxFileSize = 10 * 1024 * 1024; // 10 MB
    private static readonly int MaxDaysOld = 7; // 7 days old    
    private static readonly string LogDirectoryPath = Path.Combine(Application.dataPath, "Editor/Logs");

    static LogFileManager()
    {
        BaseUtilityMethods.DoesDirectoryExist(LogDirectoryPath);
        ResetLogFilesOnPlay();
    }

    private static void ResetLogFilesOnPlay()
    {
        try
        {
            foreach (string file in Directory.GetFiles(LogDirectoryPath, "*_Log_*.txt"))
            {
                FileInfo fileInfo = new FileInfo(file);
                if (DateTime.Now - fileInfo.LastWriteTime > TimeSpan.FromDays(MaxDaysOld))
                {
                    File.Delete(file);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to clean upold log files: {ex.Message}");
        }
    }

    public string GetLogFilePath()
    {
        string timeStamp = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
        string baseFileName = $"{timeStamp}_Log";
        int index = 0;

        while (true)
        {
            string fileName = $"{baseFileName}_{index:000}.txt";
            string filePath = Path.Combine(LogDirectoryPath, fileName);

            if (!File.Exists(filePath) || new FileInfo(filePath).Length < MaxFileSize)
            {
                return filePath;
            }

            index++;
        }
    }

    public void CheckFileSizeAndRotate(string logFilePath)
    {
        // If file exists and is larger than MaxFileSize, rename it.
        if (File.Exists(logFilePath) && new FileInfo(logFilePath).Length >= MaxFileSize)
        {
            File.Move(logFilePath, $"{logFilePath}.old");
        }
    }
}
