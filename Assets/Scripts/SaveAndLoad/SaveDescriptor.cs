using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SaveDescriptor
{
    /// <summary>
    /// unity doesnt support default implementation of interfaces, implementation of this method should be
    ///  Save.AddSelfToSaveDescriptorsList(this);
    /// </summary>
    public void AddSelfToSaveDescriptorsList();

    /// <summary>
    /// when implementing this method you should add fields to Save.SaveData
    /// then store your data in the passed in saveData struct in the feilds you created.
    /// </summary>
    /// <param name="saveData"></param>
    public void GetSaveDescriptorData(ref Save.SaveDescriptorData saveData);
}
