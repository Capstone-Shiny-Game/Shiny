using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Save
{
    public static List<Savable> savables { get; private set; }
    public struct SaveData
    {
        //player data
        public Inventory playerinventory;
        public int i;
        
        //quest data

    }

    public static void AddSelfToSavablesList(Savable self)
    {
        if (Save.savables is null)
        {
            Save.savables = new List<Savable>();
        }
        Save.savables.Add(self);
    }

    public static void SaveDataJson(string filename) 
    {
        SaveData saveData = new SaveData();
        string filepath = ConstructFilePath(filename);
        foreach (Savable savableobj in savables) {
            savableobj.GetSaveData(ref saveData);
        }
        WriteToFile(filepath, saveData);

    }
    public static void LoadDataJson(string filename)
    {
        string filepath = ConstructFilePath(filename);
        if (!File.Exists(filepath)) {
            Debug.Log("save file not found at " + filepath);
            return;
        }
        Debug.Log("save found at " + filepath);
        SaveData saveData = ReadFromFile(filepath);
        foreach (Savable savableobj in savables)
        {
            savableobj.LoadData(ref saveData);
        }
    }

    public static List<string> GetSaveFileNames() {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath+ "/", " *.json");
        List<string> fileNames = new List<string>();
        foreach (string filePath in filePaths) {
            fileNames.Add(FilenameFromFilePath(filePath));
        }
        return fileNames;
    }

    /// <summary>
    /// takes a file name and creates a platform specific filepath that includes the file name and ends in .json
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private static string ConstructFilePath(string filename) {
        return Application.persistentDataPath + "/" + filename + ".json";
    }

    /// <summary>
    /// takes a file name and creates a platform specific filepath that includes the file name and ends in .json
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private static string FilenameFromFilePath(string filePath)
    {
        return filePath.Substring(Application.persistentDataPath.Length + 1, filePath.Length - 6);
    }

    /// <summary>
    /// saves data with the Json File Format at the specified filepath
    /// </summary>
    /// <param name="filepath">the full path of the file, conventionally ends in .json</param>
    /// <param name="saveData">object or struct to be serialized and saved</param>
    private static void WriteToFile(string filepath, SaveData saveData) {
        string saveJSON = JsonUtility.ToJson(saveData);
        Debug.Log(saveJSON);
        using (StreamWriter sw = new StreamWriter(filepath))
        {
            sw.WriteLine(saveJSON);
        }
    }

    private static SaveData ReadFromFile(string filepath) {
        string saveJSON = "";
        using (StreamReader sr = new StreamReader(filepath))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                saveJSON += line;
            }
        }
        SaveData saveData = JsonUtility.FromJson<SaveData>(saveJSON);
        return saveData;
    }
    
}
