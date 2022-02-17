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
    settingsMenu,
    flightui,
    questsMenu,
    wait,
}
public class MenuManager : MonoBehaviour
{
    List<MenuContainer> menuContainers;
    public GameObject onAllPauseMenus;
    private MenuContainer currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuContainers = GetComponentsInChildren<MenuContainer>().ToList();
        currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
    }

    public void SwitchMenu(MenuType menuType, string MenuSetting = "") {
        switch (menuType)//Set Timescale and Scene
        {
            case MenuType.flightui:
                DisablePause();
                break;
            case MenuType.mainMenu:
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
        currentMenu = menuContainers.Find(x => x.menuType == menuType);
        currentMenu.gameObject.SetActive(true);
        //call setup on new menu
        currentMenu.AfterEnableSetup(menuType, MenuSetting);
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

