using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



[Serializable]
public enum MenuType
{
    mainMenu,
    inventorymenu,
    saveMenu,
    loadMenu,
    settingsMenu,
    flightui,
    questsMenu,
    uncompletedQuestsMenu,
    wait,
}
public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public static MenuManager instance;
    List<MenuContainer> menuContainers;
    public GameObject onAllPauseMenus;
    private MenuContainer currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (instance is null) {
            instance = this;
        }
        menuContainers = GetComponentsInChildren<MenuContainer>().ToList();
        currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
    }
    /// <summary>
    /// called to change the current active menu
    /// </summary>
    /// <param name="menuType">type of the new menu</param>
    public void SwitchMenu(MenuType menuType) {
        switch (menuType)//Set Timescale and Scene
        {
            case MenuType.flightui://leaving pause menu and returning to normal time
                DisablePause();
                break;
            case MenuType.mainMenu://back to main menu reset to defaults
                DisablePause();
                SceneManager.LoadScene("MainMenu");
                currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
                return;
            default:
                EnablePause();
                break;
        }
        //replace current Menu
        currentMenu.DisableSelf();
        currentMenu = findNextMenu(menuType);
        currentMenu.gameObject.SetActive(true);
        //call setup on new menu
        currentMenu.AfterEnableSetup(menuType);
    }

    private MenuContainer findNextMenu(MenuType menuType)
    {
        switch (menuType)
        {
            case MenuType.uncompletedQuestsMenu:
                return menuContainers.Find(x => x.menuType == MenuType.questsMenu);
            case MenuType.loadMenu:
                return menuContainers.Find(x => x.menuType == MenuType.saveMenu);
            default:
                return menuContainers.Find(x => x.menuType == menuType);
        }
    }

    private void DisablePause()
    {
        Time.timeScale = 1f; // set time back to normal
        onAllPauseMenus.SetActive(false);
    }

    private void EnablePause()
    {
        Time.timeScale = 0f;
        onAllPauseMenus.SetActive(true);
    }
}

