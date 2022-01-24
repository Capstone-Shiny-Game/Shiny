using System.Collections.Generic;

public class DSGroupErrorData
{
    public DSErrorData errorData { get; set; }
    public List<DSGroup> Groups { get; set; }

    public DSGroupErrorData()
    {
        errorData = new DSErrorData();
        Groups = new List<DSGroup>();
    }
}
