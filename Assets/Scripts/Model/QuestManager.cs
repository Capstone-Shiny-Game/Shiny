using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class QuestManager
{
    private static readonly List<string> active = new List<string>();
    private static readonly List<string> completed = new List<string>();

    private static readonly Dictionary<string, string[]> questItems = new Dictionary<string, string[]>();
    private static readonly Dictionary<string, string> questNPCs = new Dictionary<string, string>();

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

    public static void StartQuest(string quest, string npc = null, IEnumerable<string> items = null, int expected = 0)
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
                if (quest == "Flower Power")
                {
                    // quest requires delivery of one of any flower
                    questItems[quest] = new string[] { "Flower" };
                }
                else if (quest == "Potion of Courage")
                {
                    // delivery count is a lie
                    questItems[quest] = items.Select(TrimAndExpandName).ToArray();
                }
                else if (expected > 1 && items.Count() == 1)
                {
                    // quest requires more than one of a single item
                    questItems[quest] = new string[] { $"{TrimAndExpandName(items.Single())} (x{expected})" };
                }
                else if (expected > items.Count())
                {
                    questItems[quest] = items.Select(TrimAndExpandName).Append($"(x{expected} total items)").ToArray();
                }
                else
                {
                    questItems[quest] = items.Select(TrimAndExpandName).ToArray();
                }
            }
        }
    }

    public static void StrikeItem(string quest, string item)
    {
        quest = ExpandName(quest);
        if (quest == "Witch Quest NPC")
        {
            // quest name is also a lie
            quest = "Potion of Courage";
        }
        item = TrimAndExpandName(item);
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

    public static void CompleteQuest(string quest)
    {
        quest = ExpandName(quest);
        active.Remove(quest);
        if (!completed.Contains(quest))
        {
            completed.Add(quest);
            if (questItems.ContainsKey(quest))
            {
                string[] items = questItems[quest];
                for (int i = 0; i < items.Length; i++)
                {
                    string item = items[i];
                    if (!item.StartsWith("<s>"))
                    {
                        items[i] = $"<s>{item}</s>";
                    }
                }
            }
        }
    }

    public static string DescribeQuest(string quest)
    {
        if (questNPCs.ContainsKey(quest))
        {
            return $"* {questNPCs[quest]} needs {HumanizeArray(questItems[quest])}";
        }
        else
        {
            return "* Hot air balloon time trial";
        }
    }

    public static string ExpandName(string name) => Regex.Replace(name, "([a-z])([A-Z0-9\\(])", "$1 $2");

    private static string TrimAndExpandName(string name) => ExpandName(name.Substring(0, name.IndexOf('_')));

    private static string HumanizeArray(string[] arr)
    {
        if (arr.Length > 2)
        {
            return string.Join(", ", arr, 0, arr.Length - 1) + ", and " + arr[arr.Length - 1];
        }
        else if (arr.Length == 2)
        {
            return arr[0] + " and " + arr[1];
        }
        else
        {
            return arr.FirstOrDefault();
        }
    }
}
