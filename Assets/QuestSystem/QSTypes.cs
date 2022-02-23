using System;
using System.Collections.Generic;

public static class QS
{
    public static readonly List<(string, Type)> NodeTypes = new List<(string, Type)>() {
        ("Quest/Super Amazing Quest Type", typeof(DeleteThisQSExampleQuestNodeSO)),
        ("Action/Awesome Quest Helper Action", typeof (DeleteThisQSExampleActionNodeSO)),
        ("Utility/Start", typeof (QSStartNodeSO)),
        ("Utility/And", typeof (QSAndNodeSO)),
        ("Utility/Or", typeof (QSOrNodeSO)),
    };
}

public enum QSNPC
{
    Witch,
}

/// <summary>
/// Whether the quest is unlocked
/// </summary>
public struct QSUnlock { bool Unlocked; }

/// <summary>
/// Links an action to a quest if the quest is unlocked
/// </summary>
public struct QSLink { bool Active; }
