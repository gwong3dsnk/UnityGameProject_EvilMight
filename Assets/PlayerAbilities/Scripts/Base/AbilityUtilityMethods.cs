using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AbilityUtilityMethods
{
    public static string FormatAbilityName(string unformattedName)
    {
        // unformattedName string is the ability name with an appended _idx# at the end which needs to be removed.
        int lastIndex = unformattedName.LastIndexOf("_");
        string formattedName = unformattedName.Substring(0, lastIndex);     

        return formattedName;   
    }
}
