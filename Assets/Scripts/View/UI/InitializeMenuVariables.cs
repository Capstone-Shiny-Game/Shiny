using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initialized menu variables at the start of the game.
/// </summary>
public class InitializeMenuVariables : MonoBehaviour
{
    public SettingsMenuContainer settingsMenu;
    [HideInInspector]
    public SwipeDetection swipeDetection;
    [HideInInspector]
    public InputController inputController;
    void Awake()
    {
        settingsMenu.settings = new Settings(settingsMenu.defaultSettings); //creates settings with previously saved settings or default if previous not found.
        inputController = GameObject.FindWithTag("Player").GetComponentInChildren<InputController>();
        swipeDetection = GetComponent<SwipeDetection>();
        inputController.OnStartTouch.AddListener(swipeDetection.SwipeStart);
        inputController.OnEndTouch.AddListener(swipeDetection.SwipeEnd);
    }
}
