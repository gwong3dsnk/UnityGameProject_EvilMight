using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace TMG_EditorTools
{
    public class ProjectSizeReportEditorWindow : EditorWindow
    {
        private string projectSize = "Calculating...";
        private string pathToCalculate = ""; // Default to Assets folder

        private string pathPrefKey;

        [MenuItem("Tools/TMG_EditorTools/Project Size Report")]
        public static void ShowWindow()
        {
            var window = GetWindow<ProjectSizeReportEditorWindow>("Project Size Report");
            window.Initialize();
            window.RefreshProjectSize();
        }

        private void Initialize()
        {
            // Create a preference key using the project's name
            pathPrefKey = $"ProjectSizeReport_LastPath_{Application.productName}";

            // Load the path from EditorPrefs, defaulting to the project's root directory (Application.dataPath minus 'Assets')
            string defaultPath = Application.dataPath.Replace("/Assets", "");
            pathToCalculate = EditorPrefs.GetString(pathPrefKey, defaultPath);
        }

        private void OnGUI()
        {
            GUILayout.Label("Enter Project Path:", EditorStyles.boldLabel);
            pathToCalculate = EditorGUILayout.TextField(pathToCalculate);

            if (GUILayout.Button("Calculate Size"))
            {
                if (Directory.Exists(pathToCalculate))
                {
                    EditorPrefs.SetString(pathPrefKey, pathToCalculate); // Save path to EditorPrefs
                    RefreshProjectSize();
                }
                else
                {
                    projectSize = "Path does not exist!";
                    Repaint();
                }
            }

            GUILayout.Label("Directory Size:", EditorStyles.boldLabel);
            GUILayout.Label(projectSize, EditorStyles.largeLabel);
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(pathToCalculate))
            {
                Initialize();
            }
            RefreshProjectSize();
        }

        private void RefreshProjectSize()
        {
            projectSize = CalculateProjectSize();
            Repaint();
        }

        private string CalculateProjectSize()
        {
            if (!Directory.Exists(pathToCalculate))
                return "Invalid path";

            long totalSize = GetDirectorySize(pathToCalculate);
            return FormatBytes(totalSize);
        }

        private long GetDirectorySize(string dirPath)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = new DirectoryInfo(dirPath).GetFiles("*", SearchOption.AllDirectories);
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            return size;
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}