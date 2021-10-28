using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject camPlacement;
    public GameObject crow;

    public Camera cam;
    private bool toggleFirstPersonCam = false;
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        RotateCamera();
    }

    private void RotateCamera()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");

        //locks the cursor so it doesn't get off the window
        //TO DO: allow user to get mouse again when pressing alt?
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButtonDown(1))
        {
            toggleFirstPersonCam = !toggleFirstPersonCam;
        }


        if (Input.GetMouseButtonDown(2) || !toggleFirstPersonCam)
        {
          
            this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
        }
        
        if (toggleFirstPersonCam)
        {
            cam.transform.Rotate(-rotateVertical, 0f, 0f, Space.Self);
            cam.transform.Rotate(0f, rotateHorizontal, 0f, Space.World);
            cam.transform.position = crow.transform.position;
            crow.SetActive(false);
        }
        else
        {
            crow.SetActive(true);
            MoveDynamicCamera();
        }
    }
    private void MoveDynamicCamera()
    {

        //don't snap camera immediately to player
        float bias = 0.90f;
        //Move the camera away if the player is faster
        float distance = 15f;
        Vector3 delta = transform.position - transform.forward * distance + Vector3.up * 1.5f;

        //if (Math.Abs(transform.rotation.x) < .2f && Math.Abs(transform.forward.y) < 0.3f && !isBouncing)
        //  delta += transform.forward * distance * 1f;
        Vector3 destination = cam.transform.position * bias + delta * (1.0f - bias);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, 0.01f);
        Vector3 relativePos = transform.position - cam.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up); //camera.LookAt will spin the camera if player is looking directly up or down
        cam.transform.rotation = rotation;
    }
}
