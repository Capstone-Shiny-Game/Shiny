using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [HideInInspector]public Settings settings;
    public Slider musicVolumeSlider;
    public Slider dialogueVolumeSlider;
    public SettingsData defaultSettings = new SettingsData {musicVolume = .50f, dialogueVolume = .50f, lefthanded = false};
    private SettingsData prefferedSettings;
    private void OnEnable()
    {
        prefferedSettings = Settings.settingsData;
        //Debug.Log("music vol : " + Settings.settingsData.musicVolume);
        //Debug.Log("dialogue vol : " + Settings.settingsData.dialogueVolume);
        musicVolumeSlider.value = Settings.settingsData.musicVolume;
        dialogueVolumeSlider.value = Settings.settingsData.dialogueVolume;
        musicVolumeSlider.onValueChanged.AddListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.AddListener(changeDialogueVolume);
    }

    private void changeMusicVolume(float volume) {
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.musicVolume = volume;
        Settings.UpdateSettingData(updatedSettingsData);
    }
    private void changeDialogueVolume(float volume)
    {
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.dialogueVolume = volume;
        Settings.UpdateSettingData(updatedSettingsData);
    }

    public void SaveSettings() {
        prefferedSettings = Settings.settingsData;
    }

    public void DefaultSettings()
    {
        Settings.UpdateSettingData(defaultSettings);
        //Debug.Log("music vol : " + Settings.settingsData.musicVolume);
        //Debug.Log("dialogue vol : " + Settings.settingsData.dialogueVolume);
    }

    private void OnDisable()
    {
        Settings.UpdateSettingData(prefferedSettings);
        musicVolumeSlider.onValueChanged.RemoveListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.RemoveListener(changeDialogueVolume);
    }
}
