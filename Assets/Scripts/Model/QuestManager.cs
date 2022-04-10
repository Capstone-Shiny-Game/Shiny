using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class QuestManager
{
    private static readonly List<string> active = new List<string>();
    private static readonly List<string> completed = new List<string>();

    private static readonly Dictionary<string, string[]> questItems;
    private static readonly Dictionary<string, string> questNPCs;
    private static readonly Dictionary<string, string> questRecordedDialgoues;

    public static IEnumerable<string> ActiveQuests => active;
    public static IEnumerable<string> CompletedQuests => completed;

    public static string OldestActiveQuest
    {
        get
        {
            if (active.Count > 0)
            {
                return active[0];
            }
            else if (completed.Count > 0)
            {
                return "Have Fun!";
            }
            else
            {
                return "Explore!";
            }
        }
    }

    public static void StartQuest(string quest, string npc = null, IEnumerable<string> items = null)
    {
        quest = ExpandName(quest);
        if (!completed.Contains(quest) && !active.Contains(quest))
        {
            active.Add(quest);
            if (!string.IsNullOrEmpty(npc))
            {
                questNPCs[quest] = npc;
            }
            if (items != null)
            {
                questItems[quest] = items.Select(ExpandAndTrimName).ToArray();
            }
            questRecordedDialgoues[quest] = "";
        }
    }

    public static void StrikeItem(string quest, string item)
    {
        quest = ExpandName(quest);
        item = ExpandAndTrimName(item);
        if (questItems.ContainsKey(quest))
        {
            string[] items = questItems[quest];
            int i = Array.IndexOf(items, item);
            if (i > -1)
            {
                items[i] = $"<s>{item}</s>";
            }
        }
    }

    public static void RecordDialogue(string quest, string NPC, string dialogue)
    {
        quest = ExpandName(quest);
        if (questRecordedDialgoues.ContainsKey(quest))
        {
            questRecordedDialgoues[quest] += $"{NPC}: {dialogue}";
        }
    }

    public static void CompleteQuest(string quest)
    {
        quest = ExpandName(quest);
        active.Remove(quest);
        if (!completed.Contains(quest))
        {
            completed.Add(quest);
        }
    }

    public static string DescribeQuest(string quest)
    {
        bool isActive = active.Contains(quest);
        if (questNPCs.ContainsKey(quest))
        {
            return questNPCs[quest] + (isActive ? " needs " : " neeeded ") + string.Join(" ,", questItems[quest]);
        }
        else
        {
            return "Hot Air Balloon Time Trial";
        }
    }

    public static string GetQuestRecoredDialogue(string quest)
    {
        return questRecordedDialgoues[quest];
    }

    public static string ExpandName(string name) => Regex.Replace(name, "([a-z])([A-Z0-9\\(])", "$1 $2");

    private static string ExpandAndTrimName(string name) => ExpandName(name.Substring(0, name.IndexOf('_')));
}
