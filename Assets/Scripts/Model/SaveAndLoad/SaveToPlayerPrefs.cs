using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveToPlayerPrefs
{
    static string settingsSaveLocation = "settings";
    /// <summary>
    /// serializes and saves the settings data to player prefs at the settingsSaveLocation
    /// </summary>
    /// <param name="settingsData"></param>
    public static void SaveSettings(SettingsData settingsData)
    {
        string settingsJSON = JsonUtility.ToJson(settingsData);
        PlayerPrefs.SetString(key: settingsSaveLocation, settingsJSON);
    }
    /// <summary>
    /// checks if settings exist to be loaded
    /// </summary>
    /// <returns></returns>
    public static bool SettingsExist()
    {
        return PlayerPrefs.HasKey(key: settingsSaveLocation);
    }
    /// <summary>
    /// loads the settings data Json from player prefs at the settingsSaveLocation
    /// then deserializes it and returns it.
    /// </summary>
    /// <returns></returns>
    public static SettingsData LoadSettings()
    {
        string settingsJSON = PlayerPrefs.GetString(key: settingsSaveLocation);
        SettingsData loadedSettings = JsonUtility.FromJson<SettingsData>(settingsJSON);
        //Debug.Log("loaded music volume : " + loadedSettings.musicVolume);
        return loadedSettings;
    }
}
