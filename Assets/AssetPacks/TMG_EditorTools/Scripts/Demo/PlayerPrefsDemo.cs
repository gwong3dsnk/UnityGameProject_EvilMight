using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TMG_EditorTools
{
    public class PlayerPrefsDemo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerPrefFloat;
        [SerializeField] private TextMeshProUGUI playerPrefString;
        [SerializeField] private TextMeshProUGUI playerPrefInt;
        // Start is called before the first frame update
        void Start()
        {
            if(PlayerPrefs.GetInt("FirstDemoLoad") == 0)
            {
                PlayerPrefs.SetFloat("ChangeMe", 234.253f);
                PlayerPrefs.SetString("EditMe", "EditMyString");
                PlayerPrefs.SetInt("CheckMe", 1234);

                PlayerPrefs.SetInt("FirstDemoLoad", 1);
            }

            playerPrefFloat.text = "Pref name: ChangeMe (float) = " + PlayerPrefs.GetFloat("ChangeMe").ToString();

            playerPrefString.text = "Pref name: EditMe (string) = " + PlayerPrefs.GetString("EditMe");

            playerPrefInt.text = "Pref name: CheckMe (int) = " + PlayerPrefs.GetInt("CheckMe").ToString();
        }

    }

}
