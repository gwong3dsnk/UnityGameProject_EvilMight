using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace TMG_EditorTools
{
    public class EditorTimeTracker : EditorWindow
    {
        private DateTime startTime;
        private TimeSpan elapsedTime;
        private bool tracking;
        private string saveFilePath = "Assets/EditorTimeTracker.json";

        [MenuItem("Tools/TMG_EditorTools/Editor Timer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<EditorTimeTracker>("Editor Timer");
        }

        private void OnEnable()
        {
            LoadTime();
            tracking = true;
            EditorApplication.update += UpdateTime;
            EditorApplication.quitting += SaveTime;
        }

        private void OnDisable()
        {
            tracking = false;
            EditorApplication.update -= UpdateTime;
            EditorApplication.quitting -= SaveTime;
            SaveTime();
        }

        private void UpdateTime()
        {
            if (tracking)
            {
                elapsedTime = DateTime.Now - startTime;
                Repaint();
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("Elapsed Time: " + elapsedTime.ToString());
        }

        private void SaveTime()
        {
            var data = new EditorTimeData { elapsedTimeTicks = elapsedTime.Ticks };
            var jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(saveFilePath, jsonData);
        }

        private void LoadTime()
        {
            if (File.Exists(saveFilePath))
            {
                var jsonData = File.ReadAllText(saveFilePath);
                var data = JsonUtility.FromJson<EditorTimeData>(jsonData);
                elapsedTime = new TimeSpan(data.elapsedTimeTicks);
                startTime = DateTime.Now - elapsedTime;
            }
            else
            {
                startTime = DateTime.Now;
            }
        }


        private class EditorTimeData
        {
            public long elapsedTimeTicks;
        }
    }
}