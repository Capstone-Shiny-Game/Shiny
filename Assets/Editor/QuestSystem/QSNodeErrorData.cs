using System.Collections.Generic;

public class QSNodeErrorData
{
    public DSErrorData errorData { get; set; }
    public List<QSNode> Nodes { get; set; }

    public QSNodeErrorData()
    {
        errorData = new DSErrorData();
        Nodes = new List<QSNode>();
    }
}
