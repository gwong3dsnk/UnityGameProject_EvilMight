using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class BaseLibraryDataEditor<T> : Editor where T : ScriptableObject
{
    private GUIStyle labelTextWrapping;
    protected string abilityNameFilePath = Path.Combine(Application.dataPath, "Editor/Temp/abilityNames.txt");
    protected string abilityPrefabPath = "Assets/PlayerAbilities/Prefabs";
    protected string enumFilePath = "Assets/PlayerAbilities/Scripts/Data/AbilityNames.cs";       

    public override void OnInspectorGUI()
    {
        labelTextWrapping = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true
        };

        DrawDefaultInspector();

        // Design the GUI
        EditorGUILayout.LabelField("Ability Name Management", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Use the buttons below, in order, whenever adding/renaming/deleting ability prefabs from Assets/PlayerAbilities/Prefabs.", labelTextWrapping);

        GUIContent saveAbilityNamebuttonContent = new GUIContent("1. Save Ability Names", 
            "Click this to save the current ability name assignments");
        GUIContent buttonContent = new GUIContent("2. Regenerate Ability Names", 
            "Click this whenever adding/deleting/modifying ability prefabs in Assets/PlayerAbilities/Prefabs");
        GUIContent loadAbilityNamebuttonContent = new GUIContent("3. Load Ability Names", 
            "Click this to load the correct ability name assignments");            

        // Establish method calls on button press
        if (GUILayout.Button(saveAbilityNamebuttonContent))
        {
            SaveNamesToFile();
        }

        if (GUILayout.Button(buttonContent))
        {
            RegenerateAbilityNames();
        }

        if (GUILayout.Button(loadAbilityNamebuttonContent))
        {
            LoadAbilityNames();
        }      
    }

    protected virtual void RegenerateAbilityNames()
    {
        List<string> abilityNames = DataUtilityMethods.GetAssetPrefabNamesByPath(abilityPrefabPath);
        DataUtilityMethods.WriteNewAbilityEnums(abilityNames, enumFilePath);
        AssetDatabase.Refresh();        
    }

    protected abstract void SaveNamesToFile();

    protected abstract void LoadAbilityNames();
}
