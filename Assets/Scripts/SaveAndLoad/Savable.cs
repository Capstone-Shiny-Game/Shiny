using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Savable 
{
    /// <summary>
    /// unity doesnt support default implementation of interfaces, implementation of this method should be
    ///  Save.AddSelfToSavablesList(this);
    /// </summary>
    public void AddSelfToSavablesList();

    /// <summary>
    /// when implementing this method you should add fields to Save.SaveData
    /// then store your data in the passed in saveData struct in the feilds you created.
    /// </summary>
    /// <param name="saveData"></param>
    public void GetSaveData(ref Save.SaveData saveData);

    /// <summary>
    /// when implementing this method you should add fields to Save.SaveData
    /// then read your stored data from the passed in saveData struct in the feilds you created.
    /// this process is essentially doing the opposite of GetSaveData
    /// </summary>
    /// <param name="saveData"></param>
    public void LoadData(ref Save.SaveData saveData);
}
