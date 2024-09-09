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

        foreach (var kvp in upgradeTypeDatabase)
        {
            foreach (var type in kvp.Value)
            {
                validQueueCount++;
            }
        }

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