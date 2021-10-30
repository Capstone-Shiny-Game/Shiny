using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingController : MonoBehaviour
{

    public float speed = 4f;

    private CameraController CamController;

    // Start is called before the first frame update
    void Start()
    {
        CamController = GetComponent<CameraController>();
        CamController.isWalking = true;
        Vector3 v = transform.eulerAngles;
        v.x = 0f;
        transform.eulerAngles = v;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
