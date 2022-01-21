using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Settings settings { get; private set; }
    public Slider musicVolumeSlider;
    public Slider dialogueVolumeSlider;
    public SettingsData defaultSettings = new SettingsData {musicVolume = .50f, dialogueVolume = .50f, lefthanded = false};
    private SettingsData prefferedSettings;
    // Start is called before the first frame update
    void Start()
    {
        settings = new Settings(defaultSettings); //creates settings with previously saved settings or default if previous not found.
    }
    private void OnEnable()
    {
        prefferedSettings = Settings.settingsData;
        musicVolumeSlider.value = Settings.settingsData.musicVolume;
        dialogueVolumeSlider.value = Settings.settingsData.dialogueVolume;
        musicVolumeSlider.onValueChanged.AddListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.AddListener(changeDialogueVolume);
    }

    private void changeMusicVolume(float volume) { 

    }
    private void changeDialogueVolume(float volume)
    {

    }

    public void SaveSettings() {
        prefferedSettings = Settings.settingsData;
    }

    public void DefaultSettings()
    {
        Settings.UpdateSettingData(defaultSettings);
    }

    private void OnDisable()
    {
        Settings.UpdateSettingData(prefferedSettings);
        musicVolumeSlider.onValueChanged.RemoveListener(changeMusicVolume);
        dialogueVolumeSlider.onValueChanged.RemoveListener(changeDialogueVolume);
    }
}
