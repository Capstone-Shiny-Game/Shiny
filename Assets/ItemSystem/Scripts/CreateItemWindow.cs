using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateItemWindow : EditorWindow
{
    public static string path = "Assets/ItemSystem/ItemDatabase/";
    public static string databaseName = "itemDB";
    private static ItemDB itemDB;
    public static SerializableDictionary<string, ItemSO> items { get; set; }
    int selected = 0;
    string loadedItemType;
    string itemType;
    float weight;
    bool stackable;
    Sprite sprite;
    GameObject prefab;
    bool changedSelection = true;

    [MenuItem("Editor Tools/Add or Update Item")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateItemWindow));
    }

    void OnGUI()
    {
        // TODO: Fix corner case of renaming item to overwrite existing key
        // TODO: Add deletion
        string buttonText = "Create Item";
        itemDB = CreateAsset<ItemDB>(path, databaseName);
        items = itemDB.items;

        List<string> options = new List<string>(items.Keys);
        options.Insert(0, "Add New Item");
        int response = EditorGUILayout.Popup("Label", selected, options.ToArray());
        if (response != selected)
        {
            changedSelection = true;
        }
        selected = response;
        if (selected == 0)
        {
            GUILayout.Label("New Item Creation", EditorStyles.boldLabel);
            if (changedSelection)
            {
                itemType = "item name";
                weight = 0.0f;
                stackable = false;
                buttonText = "Create Item";
                changedSelection = false;
                sprite = null;
                prefab = null;
            }
        }
        else
        {
            GUILayout.Label("Editing Item", EditorStyles.boldLabel);
            if (changedSelection)
            {
                itemType = options[selected];
                loadedItemType = itemType;
                ItemSO item = items[itemType];
                weight = item.weight;
                sprite = item.sprite;
                prefab = item.prefab;
                buttonText = "Update Item";
                changedSelection = false;
            }
        }

        itemType = EditorGUILayout.TextField("Item Type/Name", itemType);
        weight = EditorGUILayout.FloatField("Item Weight", weight);
        sprite = (Sprite)EditorGUILayout.ObjectField("Item Sprite", sprite, typeof(Sprite), false);
        prefab = (GameObject)EditorGUILayout.ObjectField("Item Prefab", prefab, typeof(GameObject), false);
        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        if (!GUILayout.Button(buttonText))
        {
            return;
        }
        if (itemType == "item name" && weight == 0.0f && sprite is null && prefab is null)
        {
            EditorUtility.DisplayDialog("Unchanged Values",
                                        "Please update the item values to be something different",
                                        "OK");
        }
        else if (!(loadedItemType is null) && items.ContainsKey(loadedItemType))
        {
            if (EditorUtility.DisplayDialog("Existing Object",
                                            $"Replace previously created {loadedItemType} with new item {itemType}?",
                                            "Replace Item",
                                            "Cancel"))
            {
                // We need to get rid of the old item first
                if (loadedItemType is null)
                {
                    return;
                }
                items.Remove(loadedItemType);
                UpdateItems(CreateItem());
            }
        }
        else if (items.ContainsKey(itemType))
        {
            if (EditorUtility.DisplayDialog("Existing Object",
                                            $"The item type {itemType} already exists. Overwrite?",
                                            "Overwrite Item",
                                            "Cancel"))
            {
                items.Remove(itemType);
                UpdateItems(CreateItem());
            }
        }
        else
        {
            UpdateItems(CreateItem());
        }

    }

    private ItemSO CreateItem()
    {
        ItemSO itemToAdd = CreateInstance<ItemSO>();
        itemToAdd.itemType = itemType;
        itemToAdd.weight = weight;
        itemToAdd.sprite = sprite;
        itemToAdd.prefab = prefab;
        return itemToAdd;
    }

    void UpdateItems(ItemSO itemToAdd)
    {
        items.Add(itemType, itemToAdd);
        itemDB.items = items;
        SaveAsset(itemDB);
    }

    public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
    }

    private static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
    }

    private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        T asset = LoadAsset<T>(path, assetName);

        if (asset is null)
        {
            asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, fullPath);
        }

        return asset;
    }
}