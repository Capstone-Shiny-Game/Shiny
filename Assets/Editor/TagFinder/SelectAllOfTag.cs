using System.Collections;
using UnityEditor;
using UnityEngine;

public class SelectAllOfTag : ScriptableWizard
{
    public string selectedTag = "";
    int selectedLayer = 0;

    [MenuItem("Editor Tools/Select All Items of Tag")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SelectAllOfTag));
    }
    void OnGUI()
    {
        // Make a tag field so we can do dropdown stuffs
        selectedTag = EditorGUI.TagField(
            new Rect(0, 5, position.width - 10, 10),
            "New Tag:",
            selectedTag);
        // If person clicks the "Select Items" button, select those items
        if (GUI.Button(new Rect(3, 25, 90, 17), "Select Items"))
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(selectedTag);
            Selection.objects = gameObjects;
        }
    }
}