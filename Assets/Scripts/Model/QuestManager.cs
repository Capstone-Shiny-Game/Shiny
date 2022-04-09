using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class QuestManager
{
    private static readonly List<string> active = new List<string>();
    private static readonly List<string> completed = new List<string>();
    private static readonly Dictionary<string, string> descriptions = new Dictionary<string, string>();

    public static IEnumerable<string> ActiveQuests => active.Select(quest => descriptions[quest]);

    public static IEnumerable<string> CompletedQuests => completed.Select(quest => descriptions[quest]);

    public static string OldestActiveQuest
    {
        get
        {
            if (active.Count > 0)
            {
                return ExpandName(active[0]);
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

    public static void StartQuest(string quest, string npc = null, IEnumerable<string> items = null, string supplement = null)
    {
        if (!completed.Contains(quest) && !active.Contains(quest))
        {
            active.Add(quest);
            descriptions[quest] = ExpandName(quest);
            if (!string.IsNullOrEmpty(npc) && items != null)
            {
                descriptions[quest] += $"\n  ({npc} needs {string.Join(", ", items.Select(ExpandAndTrimName))})";
            }
            if (!string.IsNullOrEmpty(supplement))
            {
                descriptions[quest] += '\n' + supplement;
            }
        }
    }

    public static void CompleteQuest(string quest)
    {
        active.Remove(quest);
        if (!completed.Contains(quest))
        {
            completed.Add(quest);
        }
    }

    public static string ExpandName(string name) => Regex.Replace(name, "([a-z])([A-Z0-9\\(])", "$1 $2");

    private static string ExpandAndTrimName(string name) => ExpandName(name.Substring(0, name.IndexOf('_')));
}
