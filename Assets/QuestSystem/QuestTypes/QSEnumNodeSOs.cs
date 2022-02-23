using System.Collections.Generic;

public class QSNPCNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name += $" #{ID.Substring(ID.Length - 4)}";
        Inputs = Empty;
        Outputs = new List<QSData>() { new QSData("", typeof(QSNPC)) };
        Options = Empty;
    }
}

public class QSItemNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name += $" #{ID.Substring(ID.Length - 4)}";
        Inputs = Empty;
        Outputs = new List<QSData>() { new QSData("", typeof(ItemSO)) };
        Options = Empty;
    }
}
