using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public MenuType menuType2 = MenuType.uncompletedQuestsMenu;
    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (questMenu is null) {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }
        switch (menuType)//Set menu options for showing either completed or uncompleted quests
        {
            case MenuType.questsMenu:
                Debug.Log("showing completed quests");
                //TODO finish this
                break;
            case MenuType.uncompletedQuestsMenu:
                Debug.Log("showing uncompleted quests");
                //TODO finish this
                break;
            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }
}
