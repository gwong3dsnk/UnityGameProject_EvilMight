using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UpgradeTypesDatabase = 
    System.Collections.Generic.Dictionary<AbilityNames, 
    System.Collections.Generic.Dictionary<UpgradeTypes, 
    System.Collections.Generic.Queue<UpgradeLevelData>>>;

public class CardUtilityMethods : MonoBehaviour
{
    private static int cardIndex;    

    public static int GetNumValidLevelQueues(UpgradeTypesDatabase upgradeTypeDatabase)
    {
        int validQueueCount = 0;

        Logger.Log("Start displaying Valid Upgrades in GetNumValidLevelQueues.");
        foreach (var kvp in upgradeTypeDatabase)
        {
            foreach (var type in kvp.Value)
            {
                if (type.Value.Count > 0)
                {
                    Logger.Log($"Valid Upgrade Found - [{kvp.Key}, {type.Key}, {type.Value.Count}]");
                    validQueueCount++;
                }
            }
        }
        Logger.Log("End displaying Valid Upgrades");

        return validQueueCount;
    }    

    public static int GetCardIndex()
    {
        return cardIndex;
    }

    public static void SetCardIndex(int value)
    {
        cardIndex = value;
    }
}