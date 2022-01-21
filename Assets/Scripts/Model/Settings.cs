using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static event EventHandler OnSettingsChanged;
    public static SettingsData settingsData { get; private set; }
    
    public static void UpdateSettingData(SettingsData newSettings)
    {
        settingsData = newSettings;
        SaveToPlayerPrefs.SaveSettings(newSettings);
        OnSettingsChanged?.Invoke(newSettings, EventArgs.Empty);
    }

    public Settings(SettingsData defaultSettings) {
        if (!LoadSettings())
        {
            UpdateSettingData(defaultSettings);
        }
    }
    /// <summary>
    /// returns true if settings exist and updates settings from the save file
    /// otherwise fails and returns false
    /// </summary>
    /// <returns></returns>
    private bool LoadSettings()
    {
        if (SaveToPlayerPrefs.SettingsExist()) {
            SettingsData newSettings = SaveToPlayerPrefs.LoadSettings();
            UpdateSettingData(newSettings);
            return true;
        }
        return false;
    }
}
[Serializable]
public struct SettingsData {
    public float musicVolume;
    public float dialogueVolume;
    public bool lefthanded;
}
