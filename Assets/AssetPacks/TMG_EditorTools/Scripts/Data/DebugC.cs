using UnityEngine;
using System.Diagnostics;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace TMG_EditorTools
{
    public class DebugC
    {
        private static List<(string filePath, int lineNumber, string message)> openedScriptPositions = new List<(string, int, string)>();
        public static void Log(string message, Color color, bool bold = false, bool italic = false, int fontSize = 12)
        {
            string formattedMessage = FormatMessage(message, color, fontSize, bold, italic);
            UnityEngine.Debug.Log(formattedMessage);
            LogScriptInfo(message);
        }

        public static void LogWarning(string message, Color color, bool bold = false, bool italic = false, int fontSize = 12)
        {
            string formattedMessage = FormatMessage(message, color, fontSize, bold, italic);
            UnityEngine.Debug.LogWarning(formattedMessage);
            LogScriptInfo(message);
        }

        public static void LogError(string message, Color color, bool bold = false, bool italic = false, int fontSize = 12)
        {
            string formattedMessage = FormatMessage(message, color, fontSize, bold, italic);
            UnityEngine.Debug.LogError(formattedMessage);
            LogScriptInfo(message);
        }

        static void LogScriptInfo(string message)
        {
            StackTrace stackTrace = new StackTrace(true);
            // The frame at index 1 will be the caller of the Log* method
            StackFrame frame = stackTrace.GetFrame(2);
            string scriptFilePath = frame.GetFileName();
            int lineNumber = frame.GetFileLineNumber();


            UpdateScriptPosition(scriptFilePath, lineNumber, message);



        }

        static void UpdateScriptPosition(string filePath, int lineNumber, string message)
        {
            string relativePath = GetRelativePath(filePath);

            if (!openedScriptPositions.Any(entry => entry.filePath == relativePath && entry.lineNumber == lineNumber && entry.message == message))//check if already exists
            {
                openedScriptPositions.Add((relativePath, lineNumber, message));

            }
        }

        static string GetRelativePath(string fullPath)
        {
            int assetsIndex = fullPath.IndexOf("Assets");
            if (assetsIndex >= 0)
            {
                return fullPath.Substring(assetsIndex);
            }
            else
            {
                // If "Assets" is not found, return the full path
                return fullPath;
            }
        }

        public static List<(string filePath, int lineNumber, string message)> GetOpenedScriptPositions()
        {
            return openedScriptPositions;
        }

        static string FormatMessage(string message, Color color, int fontSize, bool bold, bool italic)
        {
            string formattedMessage = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
            if (bold)
                formattedMessage += "<b>";
            if (italic)
                formattedMessage += "<i>";

            formattedMessage += $"<size={fontSize}>{message}</size>";

            if (italic)
                formattedMessage += "</i>";
            if (bold)
                formattedMessage += "</b>";

            formattedMessage += "</color>";

            return formattedMessage;
        }
    }
}