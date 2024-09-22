using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtilityMethods
{
    public static GameObject[] GetPlayerSockets()
    {
        GameObject[] playerSockets = GameObject.FindGameObjectsWithTag("PlayerSocket");
        Logger.Log("------- START Logging out Player SOckets --------");
        foreach (var item in playerSockets) // log
        {
            Logger.Log(item.name);
        }
        Logger.Log("------- END Logging out Player SOckets --------");     

        return playerSockets;
    }
}
