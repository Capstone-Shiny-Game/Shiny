using System;
using System.Collections.Generic;

public static class QS
{
    public static readonly List<(string, Type)> NodeTypes = new List<(string, Type)>() {
        ("Utility/Start", typeof (QSStartNodeSO)),
        ("Utility/And", typeof (QSAndNodeSO)),
        ("Utility/Or", typeof (QSOrNodeSO)),
    };
}

public enum QSNPC
{
    Witch,
}

public enum QSItem
{
    Apple,
}

public struct QSUnlock { bool Unlocked; }
public struct QSLink { bool Active; }
