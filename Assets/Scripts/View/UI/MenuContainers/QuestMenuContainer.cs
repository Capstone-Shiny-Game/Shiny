using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public override void AfterEnableSetup(MenuType currentMenuType, string menuSetting)
    {
        base.AfterEnableSetup(currentMenuType,menuSetting);
        if (questMenu is null) {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }

        if (menuSetting == "" || menuSetting == "uncompleted")
        {
            Debug.Log("showing uncompleted quests");
            //TODO finish this
        }
        else if (menuSetting == "completed")
        {
            Debug.Log("showing completed quests");
            //TODO finish this
        }
        else
        {
            Debug.Log("save menu container was passed incorrect settings");
        }
    }
}
