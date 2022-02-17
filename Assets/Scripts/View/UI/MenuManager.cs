using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;




public enum MenuType
{
    mainMenu,
    pausemenu,
    saveMenu,
    loadMenu,
    QuestsCompletedMenu,
    QuestsUncompletedMenu,
    settingsMenu,
    flightui,
    wait,
}
public class MenuManager : MonoBehaviour
{
    List<MenuContainer> menuContainers;
    private MenuContainer currentMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuContainers = GetComponentsInChildren<MenuContainer>().ToList();
        currentMenu = menuContainers.Find(x => x.menuType == MenuType.flightui);
    }

    public void SwitchMenu(MenuType menuType) {
        switch (menuType)//Set Timescale and Scene
        {
            case MenuType.flightui:
                Time.timeScale = 1f; // set time back to normal
                break;
            case MenuType.mainMenu:
                Time.timeScale = 1f; // set time back to normal
                SceneManager.LoadScene("MainMenu");
                break;
            default:
               Time.timeScale = 0f; // pause game time doesnt pass
               break;
        }
        //replace current Menu
        currentMenu.DisableSelf();
        currentMenu = menuContainers.Find(x => x.menuType == menuType);
        //call setup on new menu
        currentMenu.AfterEnableSetup(menuType);
    }

}

