using System.Collections;
using System.Collections.Generic;
using TMG_EditorTools;
using UnityEngine;

namespace TMG_EditorTools
{

    public class MoreDebugLogDemos : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            DebugC.Log("Documents Example", Color.blue, true, true, 13);
            // Loop 5 times
            for (int i = 0; i < 10; i++)
            {
                DebugC.Log("test spam message", Color.black, true, true, 12);
                // Add your code here to execute for each iteration
            }
            StartCoroutine(ExampleCoroutine());
        }
        IEnumerator ExampleCoroutine()
        {
            yield return new WaitForSeconds(2);

            DebugC.Log("Delay test", Color.grey, true, true, 13);
        }
    }

}