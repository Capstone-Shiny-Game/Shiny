using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuContainer : MenuContainer
{
    public SaveMenu saveMenu;
    public MenuType menuType2 = MenuType.loadMenu;
    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (saveMenu is null)
        {
            Debug.Log("save menu container missing reference to its self");
            return;
        }
        switch (menuType)//Set Timescale and Scene
        {
            case MenuType.saveMenu:
                saveMenu.SetSavingisEnabled(true);
                break;
            case MenuType.loadMenu:
                saveMenu.SetSavingisEnabled(false);
                break;
            default:
                Debug.Log("save menu container was passed incorrect menu type");
                break;
        }
    }
}
