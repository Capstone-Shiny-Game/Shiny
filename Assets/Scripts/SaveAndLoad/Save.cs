using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Save
{
    /// <summary>
    /// list of all savable objects that will be added to the save file
    /// </summary>
    public static List<Savable> savables { get; private set; }

    /// <summary>
    /// Struct containing all feilds that need to be saved for the game
    /// struct should be edited when Savable.GetSaveData is called 
    /// with each savable object adding it's own feilds to the stuct and filling them.
    /// struct should be read when Savable.LoadData is called
    /// with each savable object reading the feilds it filled when saving.
    /// </summary>
    public struct SaveData
    {
        //player data
        public Inventory playerinventory;
        
        //quest data

    }


    /// <summary>
    /// used in Savable.AddSelfToSavablesList() to ensure that SavablesList exists and add the passed savable to the list of objects that will be saved.
    /// Savable.AddSelfToSavablesList() should be called in an awake or start function.
    /// </summary>
    /// <param name="self"></param>
    public static void AddSelfToSavablesList(Savable self)
    {
        if (Save.savables is null)
        {
            Save.savables = new List<Savable>();
        }
        Save.savables.Add(self);
    }

    /// <summary>
    /// meant to be called from the save menu,
    /// requests all savable objects in savables to save their data into the SaveData struct, 
    /// then uses the filename and the platform independant file location to save the json serialized struct to a file
    /// </summary>
    /// <param name="filename"></param>
    public static void SaveDataJson(string filename) 
    {
        SaveData saveData = new SaveData();
        string filepath = ConstructFilePath(filename);
        foreach (Savable savableobj in savables) {
            savableobj.GetSaveData(ref saveData);
        }
        WriteToFile(filepath, saveData);

    }
    /// <summary>
    /// meant to be called from the save menu, 
    /// uses the filename and the platform independant file location to get a save file and load it's data into the SaveData struct
    /// then requests all savable objects in savables to get their data from the SaveData struct.
    /// </summary>
    /// <param name="filename"></param>
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

    /// <summary>
    /// returns a list of all the names of the current save files from the platform independant file location
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// loads data from the Json File Format at the specified filepath into SaveData struct and returns it
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
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
