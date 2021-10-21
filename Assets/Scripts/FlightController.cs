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
    float pitchAngle = .45f;
    float tiltAngle = 60f;

    public Camera cam;
    public float speed = 15.0f;
    public float brake = 0.0f;
    public float glideThreshold = 0.2f;
    public float acceleration = 20.0f;

    // Time to move back from the tilted position, in seconds.
    private float smoothTilt = 2.0f;

    // The time at which the animation started.
    private float start = -1;
    private bool hasTilted = false;

    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        Fly();
    }
    void LateUpdate()
    {
        MoveCamera(); //moving the camera during update causes jitter
    }

    private void MoveCamera()
    {
        //don't snap camera immediately to player
        float bias = 0.96f;
        //Move the camera away if the player is faster
        Vector3 delta = transform.position - transform.forward * 22.0f + Vector3.up * 3.0f;
        Vector3 destination = cam.transform.position * bias + delta * (1.0f - bias);

        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref velocity, 0.01f);
        Vector3 relativePos = transform.position - cam.transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up); //camera.LookAt will spin the camera if player is looking directly up or down
        cam.transform.rotation = rotation;
    }
    private void Brake()
    {
        if (Input.GetKey(KeyCode.LeftControl) && speed > 5f) //don't brake if speed negative
            brake += 0.01f;
        else
            brake -= 0.01f;
        brake = Mathf.Clamp(brake, 0f, 5f);
        if (brake > 0 && speed > 0)
            speed = Mathf.Clamp(speed, 1f, 100f);
        else
            speed = Mathf.Clamp(speed, -5f, 100f);
        speed -= brake;
    }
    private void Fly()
    {
        //set the acceleration based on if bird is pointed up or down
        //don't alter speed if relatively straight
        if (Math.Abs(transform.forward.y) > glideThreshold) 
            speed -= transform.forward.y * Time.deltaTime * acceleration;
        else
        {
            //if straightened out, set the speed to a set velocity
            speed = Mathf.Clamp(speed, 20f, 100f);
        }
        Brake();
        transform.position += transform.forward * Time.deltaTime * speed;         //once flying, the bird is always moving

        
        // Rotate
        float turn = Input.GetAxis("Horizontal") * tiltAngle / 1.5f * Time.deltaTime;
        float pitch = -Input.GetAxis("Vertical") * pitchAngle;
        float tilt = -Input.GetAxis("Horizontal") * tiltAngle * Time.deltaTime;
        transform.Rotate(new Vector3(pitch, turn, tilt));

        ClampRotations();

        if (tilt != 0)
            hasTilted = true;
        if (tilt == 0 && hasTilted) //if no longer turning, then reset the tilt to 0
        {
            if (start < 0)
            {
                start = Time.time;
                smoothTilt = Math.Abs(Math.Min(transform.rotation.z, 360f - transform.rotation.z)) * 15f;
            }
            float fracComplete = (Time.time - start) / smoothTilt;
            DampenAngleToZero("z", fracComplete);
        }
        CheckCollisions();
    }
    private void CheckCollisions()
    {
        float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if(terrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, terrainHeight+0.1f, transform.position.z);
        }
    }
    private void ClampRotations()
    {
        // Clamp Tilt
        float angle = transform.rotation.eulerAngles.z;
        float angleX = transform.rotation.eulerAngles.x;
        //tilted too far left
        if (angle >= 45f && angle < 180f)
        {
            float diff = angle - 45f;
            transform.Rotate(new Vector3(0, 0, -diff));
        }
        else if (angle < 315f && angle >= 180f)
        {
            float diff = angle - 315f;
            transform.Rotate(new Vector3(0, 0, -diff));
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
    }
    private void DampenAngleToZero(string angle, float fracComplete)
    {

        Quaternion target = Quaternion.identity;

        if (angle.ToLower() == "x")
            target = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        else if (angle.ToLower() == "y")
            target = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        else if (angle.ToLower() == "z")
            target = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, target, fracComplete);

        float diff = transform.rotation.eulerAngles.z - target.eulerAngles.z;
        float degree = 1f;
        if (Mathf.Abs(diff) <= degree) //close enough to straight - reset damping
        {
            start = -1;
            hasTilted = false;
        }
    }
}
