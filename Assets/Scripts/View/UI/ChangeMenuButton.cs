using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ChangeMenuButton : MonoBehaviour
{
    public MenuType MenuToGoTo;
    private Button MenuButton;
    private void OnEnable()
    {
        MenuButton = GetComponent<Button>();
        MenuButton.onClick.RemoveAllListeners();
        MenuButton.onClick.AddListener(OnButtonClicked);
    }

    public void OnButtonClicked() {
        switch(MenuToGoTo)
        {
            case MenuType.mainMenu:
                AkSoundEngine.PostEvent("buttonClick", gameObject);
                break;

            case MenuType.flightui:
                AkSoundEngine.PostEvent("menuExit", gameObject);
                break;

            default:
                AkSoundEngine.PostEvent("switchTabs", gameObject);
                break;
        }

        MenuManager.instance.SwitchMenu(MenuToGoTo);
    }
}
