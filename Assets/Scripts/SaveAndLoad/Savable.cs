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
    
    public void GetSaveData(ref Save.SaveData saveData);

    public void LoadData(ref Save.SaveData saveData);
}
