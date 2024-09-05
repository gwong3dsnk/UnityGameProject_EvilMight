using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public abstract class BaseLibraryDataEditor<T> : Editor where T : ScriptableObject
{
    protected string abilityNameFilePath = Path.Combine(Application.dataPath, "Editor/Temp/abilityNames.txt");

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        T libraryData = (T)target;

        // Design the GUI
        EditorGUILayout.LabelField("Ability Name Management", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Use the buttons below to manage ability names.");        

        GUIContent saveAbilityNamebuttonContent = new GUIContent("Save Ability Names", 
            "Click this to save the current ability name assignments");
        GUIContent buttonContent = new GUIContent("Regenerate Ability Names", 
            "Click this whenever adding/deleting/modifying ability prefabs in Assets/PlayerAbilities/Prefabs");
        GUIContent loadAbilityNamebuttonContent = new GUIContent("Load Ability Names", 
            "Click this to load the correct ability name assignments");            

        // Establish method calls on button press
        if (GUILayout.Button(saveAbilityNamebuttonContent))
        {
            SaveNamesToFile();
        }

        if (GUILayout.Button(buttonContent))
        {
            RegenerateAbilityNames(libraryData);
        }

        if (GUILayout.Button(loadAbilityNamebuttonContent))
        {
            LoadAbilityNames();
        }      
    }

    protected abstract void SaveNamesToFile();

    protected abstract void RegenerateAbilityNames(T libraryData);

    protected abstract void LoadAbilityNames();
}
