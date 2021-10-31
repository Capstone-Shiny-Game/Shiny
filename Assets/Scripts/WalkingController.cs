using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8f;
    public float TurningSpeed = 60f;

    private CameraController cameraController;
    private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = GetComponent<CameraController>();
        cameraController.isWalking = true;
        rigidbody = GetComponent<Rigidbody>();
        // rigidbody.useGravity = true;
        Vector3 v = transform.eulerAngles;
        v.x = 0f;
        transform.eulerAngles = v;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * TurningSpeed, 0);
        transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * ForwardSpeed;
    }
}
