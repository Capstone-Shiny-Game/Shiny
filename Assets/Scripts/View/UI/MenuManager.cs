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
    completedQuestsMenu,
    activeQuestsMenu,
    wait,
}
public class MenuManager : MonoBehaviour
{
    [HideInInspector]
    public static MenuManager instance;
    List<MenuContainer> menuContainers;
    public GameObject onAllPauseMenus;
    public GameObject pauseMenuBackground;
    private MenuContainer currentMenu;
    [HideInInspector]
    public MenuType lastOpenedPauseMenu;
    public LevelLoader loader;
    private bool isPaused;

    // Start is called before the first frame update
    void Start()
    {
        if (instance is null)
        {
            instance = this;
        }
        menuContainers = GetComponentsInChildren<MenuContainer>(true).ToList();
        /*foreach (MenuContainer menuContainer in menuContainers) {
            Debug.Log(menuContainer.menuType);
        }*/
        currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
        lastOpenedPauseMenu = MenuType.activeQuestsMenu;
    }
    /// <summary>
    /// called to change the current active menu
    /// </summary>
    /// <param name="menuType">type of the new menu</param>
    /// <param name="calledByConfirm">if it is called from the confirm popUP</param>
    public void SwitchMenu(MenuType menuType, bool calledByConfirm = false)
    {
        // TODO fix bug with changing from save menu to load menu
        // Set Timescale and Scene
        switch (menuType)
        {
            // leaving pause menu and returning to normal time
            case MenuType.flightui:
                DisablePause();
                // We previously used the approach of opening the last opened menu, *however*
                // we added animation to the book menu and it's just a heckin' struggle to
                // try to get the previous opened menu to re-open, so I'm not gonna and you
                // can't make me lol (but really if you have an issue just ping me or something,
                // maybe you can think of a better idea than my smoothed-to-a-polish brain)

                // if (currentMenu.menuType != MenuType.flightui)
                // {
                //     lastOpenedPauseMenu = currentMenu.menuType;
                // }
                break;
            // back to main menu reset to defaults
            case MenuType.mainMenu:
                DisablePause();
                StartCoroutine(loader.LoadLevel("MainMenu"));
                currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
                return;
            default:
                EnablePause();
                break;
        }
        // replace current Menu
        if (!calledByConfirm && currentMenu.DisableSelf(menuType) == MenuType.wait)
        {
            // return value is used to interrupt changing menus if disable self needs to override
            return;
        }
        currentMenu = FindNextMenu(menuType);
        currentMenu.gameObject.SetActive(true);
        // call setup on new menu
        currentMenu.AfterEnableSetup(menuType);
    }
    private MenuContainer FindNextMenu(MenuType menuType)
    {
        switch (menuType)
        {
            case MenuType.saveMenu:
            case MenuType.loadMenu:
                return menuContainers.Find(x => x.menuType == MenuType.saveMenu);
            case MenuType.activeQuestsMenu:
            case MenuType.completedQuestsMenu:
                QuestMenuContainer container = menuContainers.Find(x => x.menuType == MenuType.activeQuestsMenu) as QuestMenuContainer;
                container.subMenuType = menuType;
                return container;
            default:
                return menuContainers.Find(x => x.menuType == menuType);
        }
    }

    private void DisablePause()
    {

        Time.timeScale = 1f; // set time back to normal
        onAllPauseMenus.SetActive(false);
        pauseMenuBackground.SetActive(false);
        // Enable camera controls when pause is disabled
        GameObject crow = GameObject.FindGameObjectWithTag("Player");
        // Should only have the one crow, and crow should only have the one
        // inputcontroller cs script attached
        crow.GetComponentInChildren<InputController>().menuOpen = false;
        isPaused = false;
    }

    private void EnablePause()
    {
        Time.timeScale = 0f;
        onAllPauseMenus.SetActive(true);
        pauseMenuBackground.SetActive(true);
        Animator bookAnimator = pauseMenuBackground.GetComponentInChildren<Animator>();
        if (!isPaused)
        {
            bookAnimator.SetTrigger("Open");
            isPaused = true;
        }

        // Disable camera controls when pause is enabled
        GameObject crow = GameObject.FindGameObjectWithTag("Player");
        crow.GetComponentInChildren<InputController>().menuOpen = true;
    }

    public MenuType GetCurrentMenuType()
    {
        return currentMenu.menuType;
    }
}
