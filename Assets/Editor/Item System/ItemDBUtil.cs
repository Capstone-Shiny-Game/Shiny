using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Some database utilities for making my life slightly easier I think perhaps
/// </summary>
public static class ItemDBUtil
{
    public static string path { get; private set; } = "Assets/ItemSystem/ItemDatabase/";
    public static string itemPath { get; private set; } = "Assets/ItemSystem/ItemSO/";
    public static string databaseName { get; private set; } = "itemDB";
    private static SerializableDictionary<string, ItemSO> items;

    /// <summary>
    /// Gets and returns the item database
    /// </summary>
    /// <returns> The item database stored in `path`</returns>
    public static ItemDB GetItemDB()
    {
        return LoadAsset<ItemDB>(path, databaseName);
    }

    /// <summary>
    /// Gets and returns the dictionary representing the item database
    /// </summary>
    /// <returns> The dict of `string`-`ItemSO` pairs found in `path`</returns>
    public static SerializableDictionary<string, ItemSO> GetItemDBSerializableDict()
    {
        return GetItemDB().items;
    }


    /// <summary>
    /// Loads and returns an asset
    /// </summary>
    public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        return AssetDatabase.LoadAssetAtPath<T>($"{path}/{assetName}.asset");
    }

    /// <summary>
    /// Saves a specified asset
    /// </summary>
    /// <param name="asset"> The asset to save </param>
    public static void SaveAsset(UnityEngine.Object asset)
    {
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        // EditorUtility.FocusProjectWindow();
    }

    /// <summary>
    /// Creates an asset with given path and name
    /// </summary>
    /// <param name="path"> Where to load the asset from if it exists, or where to save if not</param>
    /// <param name="assetName"> The name of the asset </param>
    /// <typeparam name="T"> The type of the asset </typeparam>
    /// <returns> The loaded or newly created asset </returns>
    public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
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

    /// <summary>
    /// yeets an asset out of existence
    /// </summary>
    /// <param name="path"> where to murderize the asset </param>
    /// <param name="assetName"> the name of the asset to be dead-ed </param>
    /// <typeparam name="T"> The type of the asset </typeparam>
    public static void DeleteAsset<T>(string path, string assetName) where T : ScriptableObject
    {
        string fullPath = $"{path}/{assetName}.asset";

        T asset = LoadAsset<T>(path, assetName);

        if (!(asset is null))
        {
            AssetDatabase.DeleteAsset(fullPath);
        }
    }
}
