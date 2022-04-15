using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestMenuContainer : MenuContainer
{
    public GameObject questMenu;
    public GameObject activeQuestMenu;
    public GameObject completedQuestMenu;
    public GameObject activeContainer;
    public GameObject completedContainer;

    public MenuType subMenuType = MenuType.activeQuestsMenu;

    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        if (questMenu is null)
        {
            Debug.Log("quest menu container missing reference to its self");
            return;
        }
        switch (subMenuType)//Set menu options for showing either completed or uncompleted quests
        {
            case MenuType.completedQuestsMenu:
                DestoryAllChildren(completedContainer);
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(false);
                completedQuestMenu.SetActive(true);
                foreach (string description in FormattedCompletedQuests)
                {
                    AddQuestEntry(description, completedContainer);
                }
                break;

            case MenuType.activeQuestsMenu:
                DestoryAllChildren(activeContainer);
                questMenu.SetActive(true);
                activeQuestMenu.SetActive(true);
                completedQuestMenu.SetActive(false);
                foreach (string description in FormattedActiveQuests)
                {
                    AddQuestEntry(description, activeContainer);
                }
                break;

            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }

    private void DestoryAllChildren(GameObject container)
    {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private IEnumerable<string> FormattedActiveQuests =>
        QuestManager.ActiveQuests.Select(quest => $"[ ] {quest}:\n        {QuestManager.DescribeQuest(quest)}");

    private IEnumerable<string> FormattedCompletedQuests =>
        QuestManager.CompletedQuests.Select(quest => $"[x] <s>{quest}:</s>\n        <s>{QuestManager.DescribeQuest(quest)}</s>");

    private void AddQuestEntry(string description, GameObject container)
    {
        GameObject entry = new GameObject();
        TextMeshProUGUI tmp = entry.AddComponent<TextMeshProUGUI>();
        tmp.text = description;
        tmp.fontSize = 18;
        tmp.color = Color.black;
        entry.transform.parent = container.transform;
    }
}
