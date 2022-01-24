using System.Collections.Generic;

public class QSStartNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name = "Start";
        Inputs = Empty;
        Outputs = new List<QSData>() { new QSData("Game Start", typeof(QSUnlock)) };
        Options = Empty;
    }
}

public class QSAndNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name = $"And #{ID.Substring(ID.Length - 4)}";
        Inputs = new List<QSData>() { new QSData("A", typeof(QSUnlock)), new QSData("B", typeof(QSUnlock)) };
        Outputs = new List<QSData>() { new QSData("&&", typeof(QSUnlock)) };
        Options = Empty;
    }
}

public class QSOrNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name = $"Or #{ID.Substring(ID.Length - 4)}";
        Inputs = new List<QSData>() { new QSData("A", typeof(QSUnlock)), new QSData("B", typeof(QSUnlock)) };
        Outputs = new List<QSData>() { new QSData("||", typeof(QSUnlock)) };
        Options = Empty;
    }
}
