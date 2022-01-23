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
    public ConfirmPopup confirmPopup;
    public UI_inventory UIInventory;
    public GameObject PauseMenuButtons;

    private void OnEnable()
    {
        prefferedSettings = Settings.settingsData;//save old settings 
        //Debug.Log("music vol : " + Settings.settingsData.musicVolume);
        //Debug.Log("dialogue vol : " + Settings.settingsData.dialogueVolume);
        RefreshUI();
        musicVolumeSlider.onValueChanged.AddListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.AddListener(changeDialogueVolume);
    }

    private void RefreshUI()
    {
        musicVolumeSlider.value = Settings.settingsData.musicVolume;
        dialogueVolumeSlider.value = Settings.settingsData.dialogueVolume;
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
        Settings.UpdateSettingData(defaultSettings);//TODO update sliders
        RefreshUI();
    }

    private void OnDisable()
    {
        confirmPopup.ShowPopUP("there were unsaved changes to your settings, save these new settings?", confirmNewSettings, "Save","Discard");
        this.gameObject.SetActive(false);
        musicVolumeSlider.onValueChanged.RemoveListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.RemoveListener(changeDialogueVolume);
    }
    private void confirmNewSettings(bool value)
    {
        if (value)
        {
            prefferedSettings = Settings.settingsData;
        }
        Settings.UpdateSettingData(prefferedSettings);
        UIInventory.gameObject.SetActive(true);
        PauseMenuButtons.SetActive(true);
    }
}
