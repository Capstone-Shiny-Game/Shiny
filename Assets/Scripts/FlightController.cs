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
    public Camera cam;
    public float speed = 15.0f;
    public float brake = 0.0f;
    public float glideThreshold = 0.7f;
    public float acceleration = 20.0f;
    public float maxDiveSpeed = 60f;
    public float maxGlideSpeed = 30f;
    public float stamina = 100f;
    // Time to move back from the tilted position, in seconds.
    private float smoothTilt = 2.0f;
    // The time at which the animation started.
    private float startZ = -1;
    private bool hasTilted = false;
    private bool hasPitch = false;

    private bool isBouncing = false;
    public bool isBoost = false;
    private bool isSlowing = false;
    private Vector3 endBounce;
    private float bounce;
    private Transform targetRing;

    public GameObject camPlacement;
    public GameObject crow;

    void Update()
    {
        Fly();
    }
    /// <summary>
    /// Slows down the player
    /// </summary>
    private void SlowDown()
    {
        if (Input.GetAxis("Brake") > 0 && speed > 5f) //don't brake if speed negative
            brake += 0.01f;
        else
            brake -= 0.01f;
        brake = Mathf.Clamp(brake, 0f, 5f);
        if (brake > 0 && speed > 0)
            speed = Mathf.Clamp(speed, 1f, maxDiveSpeed);
        else
            speed = Mathf.Clamp(speed, 5f, maxDiveSpeed);
        speed -= brake; //adjust the speed based on how much you're breaking
    }
    /// <summary>
    /// TO DO: Have a cooldown on boost?
    /// </summary>
    private void BoostByFlappingWings()
    {
        if (Input.GetKey(KeyCode.Space) && !isBoost)
        {
            StartBoost();
        }
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
        BoostByFlappingWings();
        if (Math.Abs(transform.forward.y) > glideThreshold)
            speed -= transform.forward.y * Time.deltaTime * acceleration;
        else
        {
            //if straightened out, set the speed to a set velocity
            speed = Mathf.Clamp(speed, 20f, maxDiveSpeed);
        }
        SlowDown();

        //magnetize the player towards boost rings
        if (isBoost && targetRing != null)
            PlayerMoveTowards(targetRing);

        //once flying, the bird is always moving. Don't continue trying to move foward if in middle of collision
        if (!isBouncing)
            transform.position += transform.forward * Time.deltaTime * speed;
        else
        {
            this.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
            this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
            StartCoroutine(MoveToPosition(endBounce, bounce / 18f));

        }

        GetPlayerControls();
    }
    private void GetPlayerControls()
    {
        // Rotate
        float turn = Input.GetAxis("Horizontal") * tiltAngle / 1.5f * Time.deltaTime;
        float pitch = -Input.GetAxis("Vertical") * pitchAngle;
        float tilt = -Input.GetAxis("Horizontal") * tiltAngle * Time.deltaTime;
        transform.Rotate(new Vector3(pitch, turn, tilt));

        if (tilt != 0)
            hasTilted = true;
        if (pitch != 0)
            hasPitch = true;

        ClampRotations(pitch);

        //if no longer turning, then reset the tilt to 0
        bool resetZ = tilt == 0 && hasTilted;
        if (resetZ) 
        {

            if (startZ < 0)
            {
                startZ = Time.time;
                smoothTilt = Math.Abs(Math.Min(transform.rotation.z, 360f - transform.rotation.z)) * 15f;
            }
            float fracComplete = (Time.time - startZ) / smoothTilt;
            DampenAngleToZero(false, false, true, fracComplete);
        }
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

    public void SetBounce(Vector3 norm)
    {
        bounce = 2.5f;
        if (norm.y >= 0.8f)//bounce the bird farther from the ground if they were flying straight down. TO DO: don't do this if the player is holding the landing button.
            bounce = 1f;
        endBounce = transform.position + norm * bounce;
        speed -= 5;

        isBouncing = true;
        Invoke("StopBounce", 0.3f);
    }

    public void SetTargetRing(Transform ringTransform)
    {
        targetRing = ringTransform;
    }

    private void PlayerMoveTowards(Transform target)
    {
        // Move our position a step closer to the target.
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

    }
    public void StartSlow()
    {

        //speed -= 15f;
        isSlowing = true;
        Invoke("StopSlow", 0.1f);

    }
    public void StartBoost()
    {
        speed += 15f;

        isBoost = true;
        Invoke("StopRingFollow", 0.25f);
    }
    void StopSlow()
    {
        isSlowing = false;
    }
    void StopBounce()
    {
        isBouncing = false;
    }
    void StopRingFollow()
    {
        targetRing = null;
        Invoke("StopBoost", 0.35f);
    }
    void StopBoost()
    {
        isBoost = false;
        if (speed > 15f)
            speed -= 10f;

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
