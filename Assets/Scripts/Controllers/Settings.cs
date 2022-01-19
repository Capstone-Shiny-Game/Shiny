using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Savable
{
    public static event EventHandler OnSettingsChanged;
    public static SettingsData settingsData { get; private set; }
    
    public static void UpdateSettingData(SettingsData newSettings)
    {
        settingsData = newSettings;
        OnSettingsChanged?.Invoke(newSettings, EventArgs.Empty);
    }
    public Settings() {
        AddSelfToSavablesList();
    }
    //TODO : use built in dict to save settings across all saves.
    public void AddSelfToSavablesList()
    {
        Save.AddSelfToSavablesList(this);
    }

    public void GetSaveData(ref Save.SaveData saveData)
    {
        saveData.settingsPackage = settingsData;
    }

    public void LoadData(ref Save.SaveData saveData)
    {
        UpdateSettingData(saveData.settingsPackage);
    }
}
[Serializable]
public struct SettingsData {
    int musicVolume;
    int dialogueVolume;
    bool lefthanded;
}
