using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{

    public GameObject Model;
    private string item;
    public void resetModelRotation()
    {
        float angle = Model.transform.rotation.eulerAngles.z;
        float diff = 0.0f;
        if (angle >= 1f && angle < 180f)
            diff = angle - 1f;

        else
            diff = angle - 359f;

        Model.transform.Rotate(new Vector3(0, 0, -diff));

        float angle2 = Model.transform.parent.rotation.eulerAngles.z;
        float diff2 = 0.0f;
        if (angle2 >= 1f && angle2 < 180f)
            diff2 = angle2 - 1f;

        else
            diff2 = angle2 - 359f;

        Model.transform.parent.Rotate(new Vector3(0, 0, -diff2));
    }
    public void setPatioItem(string name)
    {
        item = name;
    }
}
