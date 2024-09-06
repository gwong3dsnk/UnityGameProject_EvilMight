using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class Logger
{
    private static LogFileManager logFileManager;
    // private static string logFilePath = Path.Combine(Application.dataPath, "Editor/Logs/Log.txt");

    static Logger()
    {
        logFileManager = new LogFileManager();
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(string message, Object context = null)
    {
        try
        {
            string logFilePath = VerifyLogFilePath();
            WriteMessageToLogFile(logFilePath, message, "INFO");
            Debug.Log(message, context);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to write log: " + ex.Message);
        }
    }

    [Conditional("UNITY_EDITOR")]
    public static void LogWarning(string message, Object context = null)
    {
        try
        {
            string logFilePath = VerifyLogFilePath();
            WriteMessageToLogFile(logFilePath, message, "WARNING");
            Debug.LogWarning(message, context);
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to write log warning: " + ex.Message);
        }
    }    

    [Conditional("UNITY_EDITOR")]
    public static void LogError(string message, Object context = null)
    {
        try
        {
            string logFilePath = VerifyLogFilePath();
            WriteMessageToLogFile(logFilePath, message, "ERROR");
            Debug.LogError(message, context);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to write error: {ex.Message}");
        }
    }

    private static void WriteMessageToLogFile(string logFilePath, string message, string logLevel)
    {
        try
        {
            // Format message
            string timeStamp = DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            string logMessage = $"[{timeStamp}] [{logLevel}] - {message}";

            // Write to the file
            File.AppendAllText(logFilePath, logMessage + "\n");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to write message to log file: {ex.Message}");
        }
    }    
    
    private static string VerifyLogFilePath()
    {
        string logFilePath = logFileManager.GetLogFilePath();
        logFileManager.CheckFileSizeAndRotate(logFilePath);       
        return logFilePath; 
    }
}
