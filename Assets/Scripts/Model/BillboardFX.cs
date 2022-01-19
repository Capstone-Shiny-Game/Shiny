using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardFX : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * -Vector3.back,
            Camera.main.transform.rotation * -Vector3.down);
    }
}