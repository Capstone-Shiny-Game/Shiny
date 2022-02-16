using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// initialized menu variables at the start of the game.
/// </summary>
public class InitializeMenuVariables : MonoBehaviour
{
    public SettingsMenu settingsMenu;
    public SwipeDetection swipeDetection;
    [HideInInspector]
    public InputController inputController;
    // Start is called before the first frame update
    private void Start()
    {
        inputController = GameObject.FindWithTag("Player").GetComponentInChildren<InputController>();
        if (swipeDetection is null) 
        { 
            return;
        }
        inputController.OnStartTouch.AddListener(swipeDetection.SwipeStart);
        inputController.OnEndTouch.AddListener(swipeDetection.SwipeEnd);
    }
    void Awake()
    {
        settingsMenu.settings = new Settings(settingsMenu.defaultSettings); //creates settings with previously saved settings or default if previous not found.
    }
}
