using UnityEditor;
using UnityEngine;
using System;

namespace TMG_EditorTools
{
    public class WindowResizer : EditorWindow
    {
        private enum AspectState { _4x3_StandardMonitor = 0, _3x2_TV = 1, _16x10_WideMonitor = 2, _16x9_HD = 3, _5x4_LCD = 4, _21x9_UltraWideHD };
        private AspectState currentAspectState;
        private int width = 640;
        private int height = 480;
        private bool useCustomRes = false;
        private bool useHalfRes = false;

        //open window
        [MenuItem("Tools/TMG_EditorTools/Window Resizer")]
        public static void ShowWindow()
        {
            GetWindow<WindowResizer>("Window Resizer");
        }

        //this is how you display stuff in ui
        void OnGUI()
        {
            GUILayout.Label("Set Game and Scene Window Size", EditorStyles.boldLabel);

            useCustomRes = EditorGUILayout.Toggle("Use Custom Resolution", useCustomRes);
            useHalfRes = EditorGUILayout.Toggle("Use Half Resolution", useHalfRes);

            if (useCustomRes)
            {
                if(useHalfRes)
                {
                    width = EditorGUILayout.IntField("Width", width / 2);
                    height = EditorGUILayout.IntField("Height", height / 2);
                }
                else
                {

                    width = EditorGUILayout.IntField("Width", width);
                    height = EditorGUILayout.IntField("Height", height);
                }
            }
            else
            {
                currentAspectState = (AspectState)EditorGUILayout.EnumPopup("Resolution", currentAspectState);
                UpdateResolution(currentAspectState);
            }

            if (GUILayout.Button("Resize Viewport"))
            {
                ResizeGameAndSceneWindows(width, height);
            }
            if (GUILayout.Button("Make Resizable"))
            {
                MakeGameAndSceneWindowsResizable();
            }
        }

        private void UpdateResolution(AspectState resState)
        {
            if(useHalfRes)
            {
                switch (resState)
                {
                    case AspectState._4x3_StandardMonitor:
                        width = 640 / 2; height = 480 / 2; break;
                    case AspectState._3x2_TV:
                        width = 1152 / 2; height = 768 / 2; break;
                    case AspectState._16x10_WideMonitor:
                        width = 1680 / 2; height = 1050 / 2; break;
                    case AspectState._16x9_HD:
                        width = 1280 / 2; height = 720 / 2; break;
                    case AspectState._5x4_LCD:
                        width = 1280 / 2; height = 1024 / 2; break;
                    case AspectState._21x9_UltraWideHD:
                        width = 2560 / 2; height = 1080 / 2; break;//IS HALF OF ACTUAL SIZE
                }
            }
            else
            {
                switch (resState)
                {
                    case AspectState._4x3_StandardMonitor:
                        width = 640; height = 480; break;
                    case AspectState._3x2_TV:
                        width = 1152; height = 768; break;
                    case AspectState._16x10_WideMonitor:
                        width = 1680; height = 1050; break;
                    case AspectState._16x9_HD:
                        width = 1280; height = 720; break;
                    case AspectState._5x4_LCD:
                        width = 1280; height = 1024; break;
                    case AspectState._21x9_UltraWideHD:
                        width = 2560; height = 1080; break;
                }
            }
           
        }

        private void ResizeGameAndSceneWindows(int width, int height)
        {
            SetGameAndSceneWindowsSize(width, height, fixedSize: true);
        }

        private void MakeGameAndSceneWindowsResizable()
        {
            SetGameAndSceneWindowsSize(0, 0, fixedSize: false);
        }

        private void SetGameAndSceneWindowsSize(int width, int height, bool fixedSize)
        {
            // Find and resize the Game View
            var gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
            var gameView = GetWindow(gameViewType);
            if (gameView != null)
            {
                if (fixedSize)
                {
                    gameView.minSize = new Vector2(width, height);
                    gameView.maxSize = gameView.minSize;
                }
                else
                {
                    gameView.minSize = new Vector2(100, 100);
                    gameView.maxSize = new Vector2(10000, 10000);
                }
            }

            // Find and resize the Scene View
            var sceneViewType = typeof(SceneView);
            var sceneView = GetWindow(sceneViewType);
            if (sceneView != null)
            {
                if (fixedSize)
                {
                    sceneView.minSize = new Vector2(width, height);
                    sceneView.maxSize = sceneView.minSize;
                }
                else
                {
                    sceneView.minSize = new Vector2(100, 100);
                    sceneView.maxSize = new Vector2(10000, 10000);
                }
            }
        }
    }
}
