using System.Collections.Generic;

public class QSGroupErrorData
{
    public DSErrorData errorData { get; set; }
    public List<QSGroup> Groups { get; set; }

    public QSGroupErrorData()
    {
        errorData = new DSErrorData();
        Groups = new List<QSGroup>();
    }
}
