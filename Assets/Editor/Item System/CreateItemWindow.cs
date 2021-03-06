using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateItemWindow : EditorWindow
{
    public static string path = ItemDBUtil.path;
    private static string itemPath = ItemDBUtil.itemPath;
    public static string databaseName = ItemDBUtil.databaseName;
    private static ItemDB itemDB;
    public static SerializableDictionary<string, ItemSO> items { get; set; }
    int selected = 0;
    string loadedItemType = null;
    string itemType;
    float weight;
    bool stackable;
    Sprite sprite;
    GameObject prefab;
    bool changedSelection = true;
    string buttonText = "Create Item";

    [MenuItem("Editor Tools/Item Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateItemWindow));
    }

    void OnGUI()
    {
        // Loads the database, or creates it if necessary
        itemDB = ItemDBUtil.CreateAsset<ItemDB>(path, databaseName);
        items = itemDB.items;

        // The list of options that can be clicked on in the menu
        List<string> options = new List<string>(items.Keys);
        // The first one is an option to add a new item, and the rest are items
        options.Insert(0, "Add New Item");
        int response = EditorGUILayout.Popup("Label", selected, options.ToArray());
        // Update things upon changing selection
        if (response != selected)
        {
            changedSelection = true;
        }
        selected = response;
        // Adding a new item
        if (selected == 0)
        {
            // Only update upon selection being changed so as not to overwrite
            // changes actively being made
            if (changedSelection)
            {
                itemType = "item name";
                loadedItemType = null;
                stackable = false;
                weight = 0.0f;
                buttonText = "Create Item";
                changedSelection = false;
                sprite = null;
                prefab = null;
            }
            GUILayout.Label("New Item Creation", EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
        }
        // Seleted a previously existing item
        else
        {
            // Only update upon selection being changed so as not to overwrite
            // changes actively being made
            if (changedSelection)
            {
                itemType = options[selected];
                loadedItemType = itemType;
                ItemSO item = items[itemType];
                stackable = item.stackable;
                weight = item.weight;
                sprite = item.sprite;
                prefab = item.prefab;
                buttonText = "Update Item";
                changedSelection = false;
            }
            GUILayout.Label($"Editing Item {loadedItemType}", EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
        }

        itemType = EditorGUILayout.TextField("Item Type/Name", itemType);
        stackable = EditorGUILayout.Toggle("Stackable", stackable);
        weight = EditorGUILayout.FloatField("Item Weight", weight);
        sprite = (Sprite)EditorGUILayout.ObjectField("Item Sprite", sprite, typeof(Sprite), false);
        prefab = (GameObject)EditorGUILayout.ObjectField("Item Prefab", prefab, typeof(GameObject), false);
        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        if (GUILayout.Button(buttonText))
        {
            UpdateButtonClick(options);
            selected = 0;
            changedSelection = true;
        }
        if (selected != 0 && GUILayout.Button($"Delete item"))
        {
            // TODO: Make sure loadedItemType doesn't return a null reference exception
            // Haven't seen one yet, but you never know
            if (EditorUtility.DisplayDialog("Delete Object",
                                    $"Really delete {loadedItemType}?",
                                    "Delete",
                                    "Cancel"))
            {
                selected = 0;
                changedSelection = true;
                items.Remove(loadedItemType);
                ItemDBUtil.DeleteAsset<ItemSO>(itemPath, loadedItemType);
                ItemDBUtil.SaveAsset(itemDB);
            }
        }
    }

    private void UpdateButtonClick(List<string> options)
    {
        if (itemType == options[0] || (weight == 0.0f && sprite is null && prefab is null))
        {
            EditorUtility.DisplayDialog("Unchanged Values",
                                        "Please update the item values to be something different",
                                        "OK");
        }
        if (loadedItemType is null)
        {
            //didnt load item and user has changed atleast the name
            UpdateItems(CreateItem());
        }
        else if (items.ContainsKey(loadedItemType))
        {
            if (EditorUtility.DisplayDialog("Existing Object",
                                            $"Replace previously created {loadedItemType} with new item {itemType}?",
                                            "Replace Item",
                                            "Cancel"))
            {
                // We need to get rid of the old item first
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
        // item type as the name here
        ItemSO itemToAdd = ItemDBUtil.CreateAsset<ItemSO>(itemPath, itemType);
        itemToAdd.itemType = itemType;
        itemToAdd.weight = weight;
        itemToAdd.stackable = stackable;
        itemToAdd.sprite = sprite;
        // TODO: if prefab has item world component add correct type and amount there too
        itemToAdd.prefab = prefab;
        return itemToAdd;
    }

    void UpdateItems(ItemSO itemToAdd)
    {
        items.Add(itemType, itemToAdd);
        itemDB.items = items;
        ItemDBUtil.SaveAsset((UnityEngine.Object)itemToAdd);
        ItemDBUtil.SaveAsset(itemDB);
    }
}