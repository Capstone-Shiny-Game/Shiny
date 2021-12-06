using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Save
{
    public struct SaveData
    {
        private Inventory playerinventory;
        private int playerLevel;
        private int playerExp;
        private List<int> enemyHealthList;

        public SaveData(Inventory playerinventory, int playerLevel, int playerExp,
            List<int> enemyHealthList)
        {
            this.playerinventory = playerinventory;
            this.playerLevel = playerLevel;
            this.playerExp = playerExp;
            this.enemyHealthList = enemyHealthList;
        }
    }
    //static List<Savable> savables;

    public void SaveDataJson(string filename) 
    {
        string filepath = ConstructFilePath(filename);
        // for each savable call their save function and add the returned object to a list
    }
    public void LoadDataJson(string filename)
    {
        string filepath = ConstructFilePath(filename);
        
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
