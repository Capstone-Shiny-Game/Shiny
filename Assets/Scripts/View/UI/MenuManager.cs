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
    [HideInInspector]
    public MenuType lastOpenedPauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (instance is null) {
            instance = this;
        }
        menuContainers = GetComponentsInChildren<MenuContainer>(true).ToList();
        /*foreach (MenuContainer menuContainer in menuContainers) {
            Debug.Log(menuContainer.menuType);
        }*/
        currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
        lastOpenedPauseMenu = MenuType.loadMenu;
    }
    /// <summary>
    /// called to change the current active menu
    /// </summary>
    /// <param name="menuType">type of the new menu</param>
    /// <param name="calledByConfirm">if it is called from the confirm popUP</param>
    public void SwitchMenu(MenuType menuType,bool calledByConfirm = false) {
        if (currentMenu.menuType == menuType) {//do nothing we are in the correct menu
            return;
        }
        switch (menuType)//Set Timescale and Scene
        {
            case MenuType.flightui://leaving pause menu and returning to normal time
                DisablePause();
                if(currentMenu.menuType != MenuType.flightui) //catch edge cases
                { 
                    lastOpenedPauseMenu = currentMenu.menuType;
                }
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
        if (!calledByConfirm && currentMenu.DisableSelf(menuType) == MenuType.wait)
        { //return value is used to interrupt changing menus if disable self needs to override
            return;
        }
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

    public MenuType GetCurrentMenuType() {
        return currentMenu.menuType;
    }
}

