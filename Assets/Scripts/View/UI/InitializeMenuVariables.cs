using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initialized menu variables at the start of the game.
/// </summary>
public class InitializeMenuVariables : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    // Start is called before the first frame update
    void Awake()
    {
        settingsMenu.settings = new Settings(settingsMenu.defaultSettings); //creates settings with previously saved settings or default if previous not found.
    }
}
