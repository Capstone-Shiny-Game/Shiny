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
    float pitchAngle = .2f;
    float tiltAngle = 80f;
    public MeshCollider world;
    public Camera cam;
    public float speed = 15.0f;
    public float brake = 0.0f;
    public float glideThreshold = 0.5f;
    public float acceleration = 20.0f;
    public float maxDiveSpeed = 60f;
    public float maxGlideSpeed = 30f;
    public float stamina = 100f;
    // Time to move back from the tilted position, in seconds.
    private float smoothTilt = 2.0f;
    private float smoothPitch = 2.0f;
    // The time at which the animation started.
    private float startZ = -1;
    private float startY = -1;
    private bool hasTilted = false;
    private bool hasPitch = false;
    private Vector3 velocity = Vector3.zero;
    private bool slerpInProgress = false;
    private bool moveCamera = true;
    private bool isBouncing = false;
    private bool isBoost = false;
    private bool isSlowing = false;
    private Vector3 endBounce;
    private float bounce;
    // Update is called once per frame
    void Update()
    {
        Fly();
    }
    void LateUpdate()
    {
        if (moveCamera)
            MoveCamera(); //moving the camera during update causes jitter
    }

    private void MoveCamera()
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
    private void Brake()
    {
        if (Input.GetKey(KeyCode.LeftControl) && speed > 5f) //don't brake if speed negative
            brake += 0.01f;
        else
            brake -= 0.01f;
        brake = Mathf.Clamp(brake, 0f, 5f);
        if (brake > 0 && speed > 0)
            speed = Mathf.Clamp(speed, 1f, maxDiveSpeed);
        else
            speed = Mathf.Clamp(speed, -5f, maxDiveSpeed);
        speed -= brake;
    }
    private void FlapWings()
    {
        if (Input.GetKey(KeyCode.Space) && !isBoost)
        {
            StartBoost();

            //    speed += 0.02f;
            //    if (Math.Abs(transform.forward.y) < glideThreshold && speed > maxGlideSpeed)
            //        speed = Mathf.Lerp(speed, maxGlideSpeed, Time.deltaTime);
            //    else if (Math.Abs(transform.forward.y) > 0)
            //        speed += 0.05f;
            //    if (speed > 15f)
            //        stamina -= 0.2f;
        }
        //else if (stamina < 100f)
        //    stamina += 0.1f;
    }
    private IEnumerator MoveToPosition(Vector3 newPosition, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    private void Fly()
    {
        //set the acceleration based on if bird is pointed up or down
        //don't alter speed if relatively straight
        FlapWings();
        //Debug.Log(transform.forward.y);
        if (Math.Abs(transform.forward.y) > glideThreshold)
            speed -= transform.forward.y * Time.deltaTime * acceleration;
        else
        {
            //if straightened out, set the speed to a set velocity
            speed = Mathf.Clamp(speed, 20f, maxDiveSpeed);
        }
        Brake();
        //if (isSlowing && speed > 20f)
        //  speed -= 10f;
        if (!isBouncing)
            transform.position += transform.forward * Time.deltaTime * speed;         //once flying, the bird is always moving
        else
            StartCoroutine(MoveToPosition(endBounce, bounce / 20f));



        // Rotate
        float turn = Input.GetAxis("Horizontal") * tiltAngle / 1.5f * Time.deltaTime;
        float pitch = -Input.GetAxis("Vertical") * pitchAngle;
        float tilt = -Input.GetAxis("Horizontal") * tiltAngle * Time.deltaTime;
        transform.Rotate(new Vector3(pitch, turn, tilt));

        ClampRotations(pitch);

        if (tilt != 0)
            hasTilted = true;
        if (pitch != 0)
            hasPitch = true;
        bool resetZ = tilt == 0 && hasTilted;
        if (resetZ) //if no longer turning, then reset the tilt to 0
        {
            if (startZ < 0)
            {
                startZ = Time.time;
                smoothTilt = Math.Abs(Math.Min(transform.rotation.z, 360f - transform.rotation.z)) * 15f;
            }
            float fracComplete = (Time.time - startZ) / smoothTilt;
            DampenAngleToZero(false, false, true, fracComplete);
        }
        //straighten out plane if the user's close enough
    }

    private void OnCollisionEnter(Collision collision)
    {
        moveCamera = false;
        if (collision.gameObject.tag.Equals("Terrain"))
        {
            Vector3 norm = collision.GetContact(0).normal;

            bounce = 2.5f;
            if (norm.y >= 0.8f)//bounce the bird farther from the ground if they were flying straight down. TO DO: don't do this if the player is holding the landing button.
                bounce = 1f;
            endBounce = transform.position + norm * bounce;
            speed -= 5;
            Debug.Log(norm);
          /*  if (Math.Abs(norm.x) > 0 && Math.Abs(norm.y) < 0.9)
            {
                if (Input.GetAxis("Horizontal") < 0)
                    transform.Rotate(0, -45, 0);
                else
                {
                    transform.Rotate(0, 45, 0);
                }
                //transform.Rotate(0, 180, 0);
                //transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 1), norm);

            }*/
            isBouncing = true;
            Invoke("StopBounce", 0.3f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Ring") && !isBoost)
        {
            Debug.Log("RING");
            StartBoost();
        }
        else
        {
            Debug.Log("AAAAA");
            StartSlow();
        }
    }
    void StartSlow()
    {

        //speed -= 15f;
        isSlowing = true;
        Invoke("StartSlow", 0.1f);

    }
    void StopSlow()
    {
        isSlowing = false;
    }
    void StartBoost()
    {
        speed += 15f;

        isBoost = true;
        Invoke("StopBoost", 0.5f);
    }
    void StopBounce()
    {
        isBouncing = false;
    }
    void StopBoost()
    {
        isBoost = false;
        if (speed > 15f)
            speed -= 10f;
    }
    private void OnCollisionExit(Collision collision)
    {
        moveCamera = true;
    }
    private void ClampRotations(float pitch)
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
        if (pitch == 0 && hasPitch && Math.Abs(transform.forward.y) < glideThreshold)
        {
            if (angleX < 20f && angleX > 0f || angleX > 340f && angleX < 360f)
            {
                Quaternion target = transform.rotation;
                target = Quaternion.Euler(0, target.eulerAngles.y, target.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);
            }
        }



    }
    private void DampenAngleToZero(bool x, bool y, bool z, float fracComplete)
    {

        Quaternion target = transform.rotation;

        if (x)
            target = Quaternion.Euler(0, target.eulerAngles.y, target.eulerAngles.z);
        if (y)
            target = Quaternion.Euler(target.eulerAngles.x, 0, target.eulerAngles.z);
        if (z)
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, fracComplete);

        float diff = transform.rotation.eulerAngles.z - target.eulerAngles.z;
        float degree = 1f;
        if (Mathf.Abs(diff) <= degree) //close enough to straight - reset damping
        {
            startZ = -1;
            hasTilted = false;
        }


    }
}
