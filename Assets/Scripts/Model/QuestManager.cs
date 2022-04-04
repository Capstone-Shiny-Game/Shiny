// TODO (Ella) : cleanup
#define VERBOSE

using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class QuestManager
{
    private static List<string> active = new List<string>();
    private static List<string> completed = new List<string>();

    public static IEnumerable<string> ActiveQuests => active;

    public static IEnumerable<string> CompletedQuests => completed;

    public static string OldestActiveQuest => active.Count > 0 ? active[0] : "Have Fun!";

    public static void StartQuest(string quest)
    {
        quest = ExpandName(quest);
        if (!completed.Contains(quest) && !active.Contains(quest))
        {
            active.Add(quest);
        }

#if VERBOSE
        Debug.Log($"Started Quest: {quest}");
        PrintDebugInfo();
#endif

    }

    public static void CompleteQuest(string quest)
    {
        quest = ExpandName(quest);
        active.Remove(quest);
        if (!completed.Contains(quest))
        {
            completed.Add(quest);
        }

#if VERBOSE
        Debug.Log($"Completed Quest: {quest}");
        PrintDebugInfo();
#endif

    }

    private static string ExpandName(string quest) => Regex.Replace(quest, "([a-z])([A-Z])", "$1 $2");

#if VERBOSE
    private static void PrintDebugInfo()
    {
        Debug.Log("Oldest active quest (for game save):");
        Debug.Log($"-- [ ] {OldestActiveQuest}");
        Debug.Log("====");

        Debug.Log($"{active.Count} active quests:");
        foreach (string quest in active)
            Debug.Log($"-- [ ] {quest}");
        Debug.Log("====");

        Debug.Log($"{completed.Count} completed quests:");
        foreach (string quest in completed)
            Debug.Log($"-- [X] {quest}");
        Debug.Log("====");
    }
#endif

}
