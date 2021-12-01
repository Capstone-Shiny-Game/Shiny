using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public GameObject camPlacement;
    public GameObject crow;

    public Camera cam;
    public bool toggleFirstPersonCam = false;
    private Vector3 velocity = Vector3.zero;

    public bool isWalking = false;

    public float WalkingViewAngle = 30f;
    public float WalkingViewDistance = 5f;

    private bool switchPOV = false;
    private bool isCursorLocked = false;
    private float mouseX = 0.0f;
    private float mouseY = 0.0f;
    public float sensitivity = .5f;

    private float relRotX = 0.0f;
    private float relRotY = 0.0f;

    void LateUpdate()
    {
        RotateCamera();
    }


    private void RotateCamera()
    {
        float rotateHorizontal = mouseX;
        float rotateVertical = mouseY;

        //locks the cursor so it doesn't get off the window
        //TO DO: allow user to get mouse again when pressing alt?
        // if (isCursorLocked)
        // {
        //     Cursor.lockState = CursorLockMode.Locked;
        // }

        if (switchPOV)
        {
            toggleFirstPersonCam = !toggleFirstPersonCam;
            crow.SetActive(!toggleFirstPersonCam);
            if (toggleFirstPersonCam)
            {
                cam.transform.LookAt(crow.transform, Vector3.up);
                relRotX = crow.transform.rotation.eulerAngles.x;
                relRotY = crow.transform.rotation.eulerAngles.y;
            }
            switchPOV = false;
        }

        if (toggleFirstPersonCam)
        {
            LookAroundCamera(-rotateVertical, rotateHorizontal);
        }
        else
        {
            MoveDynamicCamera();
        }
    }
    private void LookAroundCamera(float x, float y)
    {
        Transform newRot = cam.transform;
        newRot.Rotate(x, 0f, 0f, Space.Self);
        newRot.Rotate(0f, y, 0f, Space.World);

        float newAngleX = newRot.rotation.eulerAngles.x;
        float newAngleY = newRot.rotation.eulerAngles.y;

        newRot.Rotate(-x, 0f, 0f, Space.Self);
        newRot.Rotate(0f, -y, 0f, Space.World);

        if (Math.Abs(newAngleX-relRotX) <= 60f || Math.Abs(newAngleX - relRotX) >= 300f)
        {
            cam.transform.Rotate(x, 0f, 0f, Space.Self);
        }
        if (Math.Abs(newAngleY - relRotY) <= 60f || Math.Abs(newAngleY - relRotY) >= 300f)
        {
            cam.transform.Rotate(0f, y, 0f, Space.World);
        }

        cam.transform.position = crow.transform.position;
    }
    private void MoveDynamicCamera()
    {
        if (isWalking)
        {
            cam.transform.position = crow.transform.position;
            cam.transform.rotation = crow.transform.rotation;
            cam.transform.Rotate(WalkingViewAngle, 0, 0);
            cam.transform.position -= cam.transform.forward * WalkingViewDistance;
        }
        else
        {
            //don't snap camera immediately to player
            float bias = 0.90f;
            //Move the camera away if the player is faster
            float distance = 4;
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

    public void OnLook(float x, float y)
    {
        mouseX = x * sensitivity;
        mouseY = y * sensitivity;
    }

    public void TogglePOV()
    {
        switchPOV = true;
    }
}