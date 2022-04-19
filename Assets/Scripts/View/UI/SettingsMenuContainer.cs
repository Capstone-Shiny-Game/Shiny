using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuContainer : MenuContainer
{
    [HideInInspector] public Settings settings;
    public Slider musicVolumeSlider;
    public Slider masterVolumeSlider;
    public Toggle invertYToggle;
    public Toggle disableAccelerometerToggle;
    public SettingsData defaultSettings = new SettingsData { musicVolume = .50f, dialogueVolume = .50f, lefthanded = false };
    private SettingsData preferredSettings;
    public ConfirmPopup confirmPopup;

    private void OnEnable()
    {
        preferredSettings = Settings.settingsData;//save old settings
        //Debug.Log("music vol : " + Settings.settingsData.musicVolume);
        //Debug.Log("dialogue vol : " + Settings.settingsData.dialogueVolume);
        RefreshUI();
        musicVolumeSlider.onValueChanged.AddListener(changeMusicVolume);
        masterVolumeSlider.onValueChanged.AddListener(changeDialogueVolume);
        invertYToggle.onValueChanged.AddListener(changeInvertY);
        disableAccelerometerToggle.onValueChanged.AddListener(changeDisableAccelerometer);

        musicVolumeSlider.value = preferredSettings.musicVolume;
        masterVolumeSlider.value = preferredSettings.dialogueVolume;

        invertYToggle.isOn = preferredSettings.invertYAxis;
        disableAccelerometerToggle.isOn = preferredSettings.disableAccelerometer;
    }

    private void Start()
    {
        if (!InputController.AccelerometerAvailable)
        {
            disableAccelerometerToggle.gameObject.SetActive(false);
        }
    }

    private void RefreshUI()
    {
        musicVolumeSlider.value = Settings.settingsData.musicVolume;
        masterVolumeSlider.value = Settings.settingsData.dialogueVolume;
    }

    private void changeMusicVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("MusicVolume", volume);
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.musicVolume = volume;
        Settings.UpdateSettingData(updatedSettingsData);
    }
    private void changeDialogueVolume(float volume)
    {
        AkSoundEngine.SetRTPCValue("MasterVolume", volume);
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.dialogueVolume = volume;
        Settings.UpdateSettingData(updatedSettingsData);
    }

    private void changeInvertY(bool invert)
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.invertYAxis = invert;
        Settings.UpdateSettingData(updatedSettingsData);
    }

    private void changeDisableAccelerometer(bool disable)
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        SettingsData updatedSettingsData = Settings.settingsData;
        updatedSettingsData.disableAccelerometer = disable;
        Settings.UpdateSettingData(updatedSettingsData);
    }

    public void SaveSettings()
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        preferredSettings = Settings.settingsData;
    }

    public void DefaultSettings()
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        Settings.UpdateSettingData(defaultSettings);//TODO update sliders
        RefreshUI();
    }
    private void confirmNewSettings(bool value, MenuType nextMenuType)
    {
        if (value)
        {
            preferredSettings = Settings.settingsData;
        }
        Settings.UpdateSettingData(preferredSettings);
        AkSoundEngine.SetRTPCValue("MasterVolume", Settings.settingsData.dialogueVolume);
        AkSoundEngine.SetRTPCValue("MusicVolume", Settings.settingsData.musicVolume);
        MenuManager.instance.SwitchMenu(nextMenuType,true);
    }

    public override MenuType DisableSelf(MenuType nextMenuType)
    {
        if (preferredSettings.Equals(Settings.settingsData)) {
            return base.DisableSelf(nextMenuType);
        }
        confirmPopup.ShowPopUP("there were unsaved changes to your settings, save these new settings?", (value) => confirmNewSettings(value,nextMenuType), "Save", "Discard");
        this.gameObject.SetActive(false);
        musicVolumeSlider.onValueChanged.RemoveListener(changeMusicVolume);
        masterVolumeSlider.onValueChanged.RemoveListener(changeDialogueVolume);
        return MenuType.wait;
    }
}
