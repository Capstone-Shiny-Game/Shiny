using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenuContainer : MenuContainer
{
    public SaveMenu saveMenu;
    public override void AfterEnableSetup(MenuType currentMenuType, string menuSetting)
    {
        base.AfterEnableSetup(currentMenuType, menuSetting);
        if (saveMenu is null)
        {
            Debug.Log("save menu container missing reference to its self");
            return;
        }

        if (menuSetting == "" || menuSetting == "save")
        {
            saveMenu.SetSavingisEnabled(true);
        }
        else if (menuSetting == "load")
        {
            saveMenu.SetSavingisEnabled(false);
        }
        else {
            Debug.Log("save menu container was passed incorrect settings");
        }
    }
}
