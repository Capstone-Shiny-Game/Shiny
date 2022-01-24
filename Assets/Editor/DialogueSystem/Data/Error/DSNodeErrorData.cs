using System.Collections.Generic;

public class DSNodeErrorData
{
    public DSErrorData errorData { get; set; }
    public List<DSNode> Nodes { get; set; }

    public DSNodeErrorData()
    {
        errorData = new DSErrorData();
        Nodes = new List<DSNode>();
    }
}
