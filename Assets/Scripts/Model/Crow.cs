using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : MonoBehaviour
{

    public GameObject Model;

    public void resetModelRotation()
    {
        float angle = Model.transform.rotation.eulerAngles.z;
        float diff = 0.0f;
        if (angle >= 1f && angle < 180f)
            diff = angle - 1f;

        else
            diff = angle - 359f;

        Model.transform.Rotate(new Vector3(0, 0, -diff));

    }

}
