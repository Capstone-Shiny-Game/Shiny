using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ItemWorld))]
public class StatListEditor : Editor
{
    private static ItemDB itemDB;
    //other item variables are in itemSO
    public static void SetItemDB()
    {
        //TODO maybe change this to it's own load function
        itemDB = CreateItemWindow.LoadAsset<ItemDB>(CreateItemWindow.path, CreateItemWindow.databaseName);
    }

    public int selectedIndex = 0;

    /// <summary>
    /// creates a list of items to choose from for item world prefabs
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SetItemDB();
        serializedObject.Update();
        ItemWorld itemWorld = (ItemWorld)target;
        EditorGUILayout.PrefixLabel("Item");
        List<string> types = new List<string>();
        foreach (string itemType in itemDB.items.Keys)
        {
            types.Add(itemType);
        }
        selectedIndex = types.IndexOf(itemWorld.item.itemType);
        selectedIndex = EditorGUILayout.Popup("ItemType", selectedIndex, types.ToArray());
        if (selectedIndex < 0)
        {
            selectedIndex = 0;
        }
        itemWorld.item.itemType = types[selectedIndex];
        if (itemWorld.item.amount < 1)
        {
            itemWorld.item.amount = 1;
        }
    }
}