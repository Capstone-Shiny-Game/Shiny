/**
 * 
 * 
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public float forwardSpd = 15f, strafeSpd = 7.5f, hoverSpd = 5f;
    private float currForwardSpd, currHoverSpd;
    float pitchAngle = .45f;
    float tiltAngle = 60f;

    public Camera cam;
    public float speed = 15.0f;
    public float brake = 0.0f;
    // Start is called before the first frame update

    public Transform start;
    public Transform end;

    // Time to move from sunrise to sunset position, in seconds.
    public float journeyTime = 1.0f;
    public float tiltTime = 2.0f;

    // The time at which the animation started.
    private float startTime = -1;
    private float startTilt = -1;

    private bool doUTurn = false;
    private bool hasTilted = false;

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Fly();
    }
    void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        // Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 3.0f;
        float bias = 0.96f;
        //cam.transform.position = cam.transform.position * bias + moveCamTo * (1.0f-bias);
        // cam.transform.LookAt(transform.position + transform.forward*20.0f);

        //Vector3 point = cam.WorldToViewportPoint(transform.position);
        //Vector3 delta = transform.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 delta = transform.position - transform.forward * 22.0f + Vector3.up * 3.0f;
        Vector3 destination = cam.transform.position * bias + delta * (1.0f - bias);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, 0.01f);
        cam.transform.LookAt(transform.position + transform.forward * 20.0f);

    }
    private void Fly()
    {
        //set the acceleration based on if bird is pointed up or down
        speed -= transform.forward.y * Time.deltaTime * 30.0f;
        if (Input.GetKey(KeyCode.LeftControl) && speed > 5f)
            brake += 0.01f;
        else
            brake -= 0.01f;
        brake = Mathf.Clamp(brake, 0f, 5f);
        if (brake > 0 && speed > 0)
            speed = Mathf.Clamp(speed, 1f, 100f);
        else
            speed = Mathf.Clamp(speed, -5f, 100f);
        speed -= brake;
        transform.position += transform.forward * Time.deltaTime * speed;         //once flying, the bird is always moving


        float turn = Input.GetAxis("Horizontal") * tiltAngle / 1.5f * Time.deltaTime;
        float pitch = -Input.GetAxis("Vertical") * pitchAngle;
        float tilt = -Input.GetAxis("Horizontal") * tiltAngle * Time.deltaTime;

        // Rotate
        transform.Rotate(new Vector3(pitch, turn, tilt));

        // Clamp Tilt
        float angle = transform.rotation.eulerAngles.z;
        float angleX = transform.rotation.eulerAngles.x;
        //Debug.Log("ANGLE " + angleX);
        //tilted too far left
        if (angle >= 45f && angle < 180f)
        {
            float diff = angle - 45f;     
            transform.rotation *= Quaternion.AngleAxis(-diff, Vector3.forward);
        }
        else if (angle < 315f && angle >= 180f)
        {
            float diff = angle -315f;
            transform.rotation *= Quaternion.AngleAxis(-diff, Vector3.forward);

        }

        //tilted too far down
        if (angleX >= 80f && angleX < 180f)
        {
            float diff = angleX - 80f;
            transform.rotation *= Quaternion.AngleAxis(-diff, Vector3.right);
        }
        else if (angleX < 280f && angleX >= 180f)
        {
            float diff = angleX - 280f;
            transform.rotation *= Quaternion.AngleAxis(-diff, Vector3.right);
        }

        if (tilt != 0)
            hasTilted = true;
        if (tilt == 0 && hasTilted) //if not pressing AD, then reset the tilt to 0
        {
            if (startTilt < 0)
            {
                startTilt = Time.time;
                tiltTime = Math.Abs(360f - transform.rotation.z);
            }
            float fracComplete = (Time.time - startTilt) / tiltTime;
            Quaternion target = DampenAngleToZero("z", fracComplete);

            float diff = transform.rotation.eulerAngles.z - target.eulerAngles.z;
            float degree = 1f;
            if (Mathf.Abs(diff) <= degree)
            {
                startTilt = -1;
                hasTilted = false;
            }
        }

    }

    private Quaternion DampenAngleToZero(string angle, float fracComplete)
    {
        Quaternion target = transform.rotation;
        target.x /= target.w;
        target.y /= target.w;
        target.z /= target.w;
        target.w = 1.0f;

        float angleZero = 2.0f * Mathf.Rad2Deg * Mathf.Atan(0);
        if(angle.ToLower() == "x")
            target.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZero);
        else if(angle.ToLower() == "y")
            target.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZero);
        else if(angle.ToLower() == "z")
            target.z = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleZero);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, fracComplete);
        return target;
    }
}
