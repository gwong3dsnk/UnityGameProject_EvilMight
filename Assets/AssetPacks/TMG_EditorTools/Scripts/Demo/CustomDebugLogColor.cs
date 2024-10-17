using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TMG_EditorTools
{

    public class CustomDebugLogColor : MonoBehaviour
    {
        private void Awake()
        {
            // Example usage
            DebugC.Log("test log", Color.green);
            DebugC.LogWarning("test warning", Color.yellow, false, true, 20);
            DebugC.LogError("test error", Color.red, true, false, 18);

        }
    }

}