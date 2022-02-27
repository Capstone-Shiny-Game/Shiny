using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowerbedScript : MonoBehaviour
{
    private MeshFilter selfFilter;
    private MeshFilter parentFilter;

    void Start()
    {
        selfFilter = GetComponent<MeshFilter>();
        parentFilter = transform.parent.GetComponent<MeshFilter>();

        selfFilter.sharedMesh = parentFilter.sharedMesh;

        DayController.OnMorningEvent += updateMesh;
    }

    private void updateMesh()
    {
        selfFilter.sharedMesh = parentFilter.sharedMesh;
    }



}
