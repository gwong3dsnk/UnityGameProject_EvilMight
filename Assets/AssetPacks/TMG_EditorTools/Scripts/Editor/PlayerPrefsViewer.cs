using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace TMG_EditorTools
{

    public class PlayerPrefsViewer : EditorWindow
    {
        private Vector2 scrollPosition;
        private List<string> playerPrefsKeys = new List<string>();

        [MenuItem("Tools/TMG_EditorTools/PlayerPrefs Viewer")]
        public static void ShowWindow()
        {
            GetWindow<PlayerPrefsViewer>("PlayerPrefs Viewer");
        }

        private void OnEnable()
        {

            SearchPlayerPrefsInAssets();
        }
        private void OnGUI()
        {
            GUILayout.Label("PlayerPrefs Viewer", EditorStyles.boldLabel);

            if (GUILayout.Button("Clear List"))
            {
                ClearList();
            }

            if (GUILayout.Button("Refresh (Find All PlayerPrefs in Assets)"))
            {
                SearchPlayerPrefsInAssets();
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            foreach (var key in playerPrefsKeys)
            {
                DisplayPlayerPref(key);
            }

            GUILayout.EndScrollView();
        }
        private void DisplayPlayerPref(string key)
        {
            EditorGUILayout.BeginHorizontal(); // Start a horizontal layout

            EditorGUI.BeginDisabledGroup(true); // First column (read-only)
            DisplayPrefValue(key);
            EditorGUI.EndDisabledGroup();

            DisplayEditablePrefValue(key); // Second column (editable)

            EditorGUILayout.EndHorizontal(); // End the horizontal layout
        }

        private void DisplayPrefValue(string key)
        {
            EditorGUI.BeginDisabledGroup(true); // Disable GUI elements to make them read-only

            // Attempt to get value as float
            float floatValue = PlayerPrefs.GetFloat(key, float.MinValue);
            if (floatValue != float.MinValue)
            {
                EditorGUILayout.TextField(key, floatValue.ToString());
            }
            else
            {
                // Attempt to get value as int
                int intValue = PlayerPrefs.GetInt(key, int.MinValue);
                if (intValue != int.MinValue)
                {
                    EditorGUILayout.TextField(key, intValue.ToString());
                }
                else
                {
                    // Default to string
                    string stringValue = PlayerPrefs.GetString(key, null);
                    if (stringValue != null)
                    {
                        EditorGUILayout.TextField(key, stringValue);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(key, "Key not found");
                    }
                }
            }

            EditorGUI.EndDisabledGroup(); // Re-enable GUI elements
        }
        private void DisplayEditablePrefValue(string key)
        {
            // Attempt to get value as float
            float floatValue = PlayerPrefs.GetFloat(key, float.MinValue);
            if (floatValue != float.MinValue)
            {
                string newFloatValue = EditorGUILayout.TextField(key, floatValue.ToString());
                if (newFloatValue != floatValue.ToString())
                {
                    float parsedValue;
                    if (float.TryParse(newFloatValue, out parsedValue))
                    {
                        PlayerPrefs.SetFloat(key, parsedValue);
                    }
                }
                return;
            }

            // Attempt to get value as int
            int intValue = PlayerPrefs.GetInt(key, int.MinValue);
            if (intValue != int.MinValue)
            {
                string newIntValue = EditorGUILayout.TextField(key, intValue.ToString());
                if (newIntValue != intValue.ToString())
                {
                    int parsedValue;
                    if (int.TryParse(newIntValue, out parsedValue))
                    {
                        PlayerPrefs.SetInt(key, parsedValue);
                    }
                }
                return;
            }

            // Default to string
            string stringValue = PlayerPrefs.GetString(key, null);
            if (stringValue != null)
            {
                string newStringValue = EditorGUILayout.TextField(key, stringValue);
                if (newStringValue != stringValue)
                {
                    PlayerPrefs.SetString(key, newStringValue);
                }
            }
            else
            {
                EditorGUILayout.LabelField(key, "Key not found");
            }
        }

        private void ClearList()
        {
            playerPrefsKeys.Clear();
        }
        private void SearchPlayerPrefsInAssets()
        {
            string[] allFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);
            Regex regex = new Regex(@"PlayerPrefs\.(GetString|GetInt|GetFloat|SetFloat|SetInt|SetBool)\(\""(.+?)\""", RegexOptions.Compiled);

            foreach (string file in allFiles)
            {
                string content = File.ReadAllText(file);
                foreach (Match match in regex.Matches(content))
                {
                    string key = match.Groups[2].Value;
                    if (!playerPrefsKeys.Contains(key))
                    {
                        playerPrefsKeys.Add(key);
                    }
                }
            }
        }
    }
}