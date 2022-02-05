using System.Collections;
using UnityEditor;
using UnityEngine;

public class SelectAllOfTag : ScriptableWizard
{
    public string searchTag = "Tag";

    [MenuItem("Editor Tools/Select All Items of Tag")]
    private static void SelectAllOfTagWizard()
    {
        ScriptableWizard.DisplayWizard<SelectAllOfTag>("Select All of Tag", "Make Selection");
    }

    private void OnWizardCreate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(searchTag);
        Selection.objects = gameObjects;
    }
}