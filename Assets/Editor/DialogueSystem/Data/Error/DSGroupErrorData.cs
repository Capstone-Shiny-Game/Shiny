using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

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
