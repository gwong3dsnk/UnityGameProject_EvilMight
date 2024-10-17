using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace TMG_EditorTools
{
    public class OpenScriptFromDebug : EditorWindow
    {
        private static OpenScriptFromDebug instance;

        [MenuItem("Tools/TMG_EditorTools/Find DebugC Scripts")]
        public static OpenScriptFromDebug CreateOrFocusWindow()
        {
            if (instance == null)
            {
                instance = GetWindow<OpenScriptFromDebug>("Find DebugC Scripts");
            }
            else
            {
                instance.Focus();
            }
            return instance;
        }

        public static void RefreshWindow()
        {
            var window = CreateOrFocusWindow();
            if (window != null)
            {
                window.Repaint(); // Refresh window to reflect changes
            }
        }

        void OnGUI()
        {
            GUILayout.Label("Script Locations (Updates On Play):", EditorStyles.boldLabel);
            foreach (var position in DebugC.GetOpenedScriptPositions())
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{position.filePath} : Line {position.lineNumber} - {position.message}");
                if (GUILayout.Button("Find", GUILayout.Width(50)))
                {
                    EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(position.filePath));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}