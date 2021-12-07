using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save
{
    public struct SaveData
    {
        private Inventory playerinventory;

    }
    //static List<Savable> savables;

    public void SaveDataJson(string filename) 
    {
        SaveData saveData = new SaveData();
        string filepath = ConstructFilePath(filename);
        foreach (Savable savableobj in Savable.savables) {
            savableobj.GetSaveData(saveData);
        }
        WriteToFile(filepath, saveData);

    }
    public void LoadDataJson(string filename)
    {
        string filepath = ConstructFilePath(filename);
        SaveData saveData = ReadFromFile(filepath);
        foreach (Savable savableobj in Savable.savables)
        {
            savableobj.LoadData(saveData);
        }
    }

    /// <summary>
    /// takes a file name and creates a platform specific filepath that includes the file name and ends in .json
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    private string ConstructFilePath(string filename) {
        return Application.persistentDataPath + "/" + filename + ".json";
    }

    /// <summary>
    /// saves data with the Json File Format at the specified filepath
    /// </summary>
    /// <param name="filepath">the full path of the file, conventionally ends in .json</param>
    /// <param name="saveData">object or struct to be serialized and saved</param>
    private static void WriteToFile(string filepath, SaveData saveData) {
        string saveJSON = JsonUtility.ToJson(saveData);
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
