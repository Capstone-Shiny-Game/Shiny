using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : Savable
{
    public Settings() {
        AddSelfToSavablesList();
    }
    public void AddSelfToSavablesList()
    {
        Save.AddSelfToSavablesList(this);
    }

    public void GetSaveData(ref Save.SaveData saveData)
    {
        saveData.settingsPackage = SettingData.settingsPackage;
    }

    public void LoadData(ref Save.SaveData saveData)
    {
        SettingData.UpdateSettingData(saveData.settingsPackage);
    }
}
[Serializable]
public struct SettingsPackage {
    int musicVolume;
    int dialogueVolume;
    bool lefthanded;
}
public static class SettingData
{
    public static event EventHandler OnSettingsChanged;
    public static SettingsPackage settingsPackage { get; private set; }

    public static void UpdateSettingData(SettingsPackage newSettings)
    {
        settingsPackage = newSettings;
        OnSettingsChanged?.Invoke(newSettings, EventArgs.Empty);
    }
}
