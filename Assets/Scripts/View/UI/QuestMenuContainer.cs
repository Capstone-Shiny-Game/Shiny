using TMPro;
using UnityEngine;

public class QuestMenuContainer : MenuContainer
{
    public GameObject activeQuestMenu;
    public GameObject completedQuestMenu;

    public RectTransform activeContainer;
    public RectTransform completedContainer;
    public RectTransform questTemplate;

    public MenuType subMenuType = MenuType.activeQuestsMenu;

    public override void AfterEnableSetup(MenuType currentMenuType)
    {
        base.AfterEnableSetup(currentMenuType);
        switch (subMenuType)
        {
            case MenuType.completedQuestsMenu:
                DestoryAllChildren(completedContainer);
                gameObject.SetActive(true);
                activeQuestMenu.SetActive(false);
                completedQuestMenu.SetActive(true);
                foreach (string quest in QuestManager.CompletedQuests)
                {
                    AddQuestEntry(quest, true);
                }
                break;

            case MenuType.activeQuestsMenu:
                DestoryAllChildren(activeContainer);
                gameObject.SetActive(true);
                activeQuestMenu.SetActive(true);
                completedQuestMenu.SetActive(false);
                foreach (string quest in QuestManager.ActiveQuests)
                {
                    AddQuestEntry(quest, false);
                }
                break;

            default:
                Debug.Log("Quest menu container was passed incorrect menu type");
                break;
        }
    }

    private void DestoryAllChildren(RectTransform container)
    {
        foreach (Transform child in container)
        {
            if (child != questTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void AddQuestEntry(string quest, bool completed)
    {
        RectTransform container = completed ? completedContainer : activeContainer;
        RectTransform entry = Instantiate(questTemplate, container);
        entry.Find("Header").GetComponent<TextMeshProUGUI>().text = quest;
        entry.Find("Body").GetComponent<TextMeshProUGUI>().text = QuestManager.DescribeQuest(quest);
        entry.Find("Checked").gameObject.SetActive(completed);
        entry.gameObject.SetActive(true);
    }
}
