using System.Collections.Generic;

public class DeleteThisQSExampleQuestNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name = $"Super Amazing Quest Type #{ID.Substring(ID.Length - 4)}";
        Inputs = new List<QSData>() {
            new QSData("Unlock", typeof(QSUnlock)),
            new QSData("Very important NPC", typeof(QSNPC)),
            new QSData("NPC wants this item", typeof(ItemSO)),
            new QSData("Another NPC?", typeof (QSNPC)),
            new QSData("Another item???", typeof (ItemSO)),
        };
        Outputs = new List<QSData>() {
            new QSData("Complete", typeof(QSUnlock)),
            new QSData("Actions", typeof(QSLink)),
        };
        Options = new List<QSData>() {
            new QSData("Quest visible to player", typeof(bool)),
            new QSData("Days of week", typeof(string)),
        };
    }
}

public class DeleteThisQSExampleActionNodeSO : QSNodeSO
{
    public override void InitializeEditor()
    {
        base.InitializeEditor();
        Name = $"Awesome Quest Helper Action #{ID.Substring(ID.Length - 4)}";
        Inputs = new List<QSData>() {
            new QSData("During this quest", typeof(QSLink)),
            new QSData("Item to spawn", typeof(ItemSO)),
        };
        Outputs = Empty;
        Options = new List<QSData>() {
            new QSData("# to spawn", typeof(string)), // ideally would be an unsigned int
        };
    }
}
