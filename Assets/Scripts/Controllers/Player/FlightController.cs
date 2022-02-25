using System;
using System.Collections;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    float pitchSensitivity = 50f;
    float tiltSensitivity = 80f;
    public float speed = 10.0f;
    public float brake = 0.0f;
    public float glideAngleThreshold = 0.7f;
    public float acceleration = 15.0f;
    public float maxDiveSpeed = 40f;
    public float minGlideSpeed = 10f;
    public float maxHeight = 100f;
    public float heightAboveLand = 25f;
    // Time to move back from the tilted position, in seconds.
    private float smoothTilt = 2.0f;
    // The time at which the animation started.
    private float startZ = -1;
    private bool hasTilted = false;
    private bool hasPitch = false;

    private bool isBouncing = false;
    public bool isBoost = false;
    private bool isSlowing = false;
    private float moveX = 0.0f;
    private float moveY = 0.0f;
    private bool isBraking = false;
    private bool isBoosting = false;
    private Vector3 endBounce;
    private float bounce;
    private Transform targetRing;
    public GameObject LeftTrail;
    public GameObject RightTrail;
    private Crow crow;
    public GameObject ceiling;
    public event Action<bool> Landed;
    public event Action<bool> FlightTypeChanged;
    public bool isGliding = false;
    private PlayerController pcontroller;
    public void Start()
    {
        //InputSystem.EnableDevice(UnityEngine.InputSystem.Gyroscope.current);
        crow = GetComponent<Crow>();
        pcontroller = GetComponent<PlayerController>();

    }

    void Update()
    {
        Fly();
        TrailScale();
        //checks speed, update boolean of isGlide if over certain limit.
        CheckSpeed();
    }
    public IEnumerator Reset(float endTime, float xRot, float height)
    {
        float elapsedTime = 0;
        while (elapsedTime < endTime)
        {
        
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), 1f); ;
            Quaternion target = transform.rotation;
            target = Quaternion.Euler(xRot, target.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, endTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }



    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mountain"))
        {
            // Debug.Log("Mountain");
            Vector3 v = other.ClosestPoint(transform.position);
            Vector3 newVector = transform.position - v;
            StartCoroutine(Slow());
            transform.LookAt(newVector);
            // StartCoroutine(BounceOnCollision(other.GetContact(0).normal));

        }
        else if (other.gameObject.CompareTag("Ceiling"))
        {
            // Debug.Log("Ceiling");
            StartCoroutine(Reset(3f, 10f, maxHeight - 5f));
           // StartCoroutine(Slow());

        }

        else if (other is TerrainCollider)
        {
            Landed?.Invoke(false);
        }
        else if (other.CompareTag("Terrain"))
        {
            Landed?.Invoke(true);
        }
        else if (other.CompareTag("Ring") && !isBoost)
        {
            //Debug.Log("RING2");
            Transform targetRing = other.gameObject.transform;
            SetTargetRing(targetRing);
            //transform.LookAt(targetRing);
            StartCoroutine(Boost());

        }
        else if (other.CompareTag("BoostBug") && !isBoost)
        {
            other.gameObject.SetActive(false);
            StartCoroutine(Boost());
        }

        else if (!other.isTrigger)
        {
            Vector3 bouncedUp = transform.position + (transform.up * 5);
            transform.TestCollision(bouncedUp, out bool collided, out _);
            Debug.Log(collided);
            if (!collided)
            {
                transform.position = bouncedUp;
                //StartCoroutine(Reset(1f, -10f, transform.position.y + 1f));

            }
            else
            {
                transform.position -= transform.forward * 2;
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 50))
                {
                    float turn = 45 * Mathf.Sign(Vector3.SignedAngle(
                        Vector3.ProjectOnPlane(transform.forward, Vector3.up),
                        Vector3.ProjectOnPlane(hit.normal, Vector3.up),
                        transform.up));
                    transform.RotateAround(transform.position, Vector3.up, turn);
                }
            }

            //StartCoroutine(Slow());
        }
    }

    /// <summary>
    /// Slows down the player
    /// </summary>
    private void SlowDown()
    {
        if ((isBraking && speed > 5f) || (isSlowing && speed > 0)) //don't brake if speed negative
            brake += 0.01f;
        else
            brake -= 0.02f;
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
        if (isBoosting && !isBoost)
        {
            StartCoroutine("Boost");
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
        if (Math.Abs(transform.forward.y) > glideAngleThreshold)
            speed -= transform.forward.y * Time.deltaTime * acceleration;
        else
        {
            //if straightened out, set the speed to a set velocity
            speed = Mathf.Clamp(speed, minGlideSpeed, maxDiveSpeed);
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
            //StartCoroutine(MoveToPosition(endBounce, bounce / 18f));

        }
        //  if (!CamController.toggleFirstPersonCam) first person
        //  {
        GetPlayerControls();
        // }
    }
    private void GetPlayerControls()
    {

        // Rotate
        float turn = moveX * tiltSensitivity / 1.5f * Time.deltaTime;
        float pitch = -moveY * pitchSensitivity * Time.deltaTime;
        float tilt = -moveX * tiltSensitivity * Time.deltaTime;
        //transform.Rotate(new Vector3(pitch, turn, 0.0f));
        transform.Rotate(0f, turn, 0f, Space.World);
        transform.Rotate(pitch, 0f, 0f, Space.Self);

        crow.Model.transform.Rotate(new Vector3(0.0f, 0.0f, tilt));

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
                smoothTilt = Math.Abs(Math.Min(crow.Model.transform.rotation.z, 360f - crow.Model.transform.rotation.z)) * 15f;
            }
            float fracComplete = (Time.time - startZ) / smoothTilt;
            DampenAngleToZero(false, false, true, fracComplete, crow.Model.transform.rotation);
        }
    }

    private void ClampRotations(float pitch)
    {
        // Clamp Tilt
        float angle = crow.Model.transform.rotation.eulerAngles.z;
        float angleX = transform.rotation.eulerAngles.x;
        //tilted too far left or right
        if (angle >= 45f && angle < 180f)
        {
            float diff = angle - 45f;
            crow.Model.transform.Rotate(new Vector3(0, 0, -diff));
        }
        else if (angle < 315f && angle >= 180f)
        {
            float diff = angle - 315f;
            crow.Model.transform.Rotate(new Vector3(0, 0, -diff));
        }
        //tilted too far down or up
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
        if (pitch == 0 && hasPitch && Math.Abs(transform.forward.y) < glideAngleThreshold)
        {
            if (angleX < 20f && angleX > 0f || angleX > 340f && angleX < 360f)
            {
                Quaternion target = transform.rotation;
                target = Quaternion.Euler(0, target.eulerAngles.y, target.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime);



            }
        }

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

    public IEnumerator BounceOnCollision(Vector3 norm)
    {
        bounce = 2.5f;
        if (norm.y >= 0.8f)//bounce the bird farther from the ground if they were flying straight down. TO DO: don't do this if the player is holding the landing button.
            bounce = 1f;
        endBounce = transform.position + norm * bounce;
        speed -= 5;
        transform.LookAt(norm);
        isBouncing = true;
        yield return new WaitForSeconds(.3f);
        isBouncing = false;
    }
    public IEnumerator Boost()
    {
        speed += 15f;
        isBoost = true;
        yield return new WaitForSeconds(3f);
        targetRing = null;
        yield return new WaitForSeconds(.35f);
        if (speed > 15f)
            speed -= 5f;
        yield return new WaitForSeconds(2f);
        isBoost = false;

    }
    public IEnumerator Slow()
    {
        isSlowing = true;
        yield return new WaitForSeconds(2f);
        isSlowing = false;

    }
    //15f is max speed, might need to play around with speed. Check with Angelique.

    public void CheckSpeed()
    {
        bool newGlide = speed > 12 && isBoost;
        if (newGlide != isGliding)
        {
            isGliding = newGlide;
            FlightTypeChanged?.Invoke(isGliding);
        }
    }

    private void DampenAngleToZero(bool x, bool y, bool z, float fracComplete, Quaternion target)
    {

        if (x)
            target = Quaternion.Euler(0, target.eulerAngles.y, target.eulerAngles.z);
        if (y)
            target = Quaternion.Euler(target.eulerAngles.x, 0, target.eulerAngles.z);
        if (z)
            target = Quaternion.Euler(target.eulerAngles.x, target.eulerAngles.y, 0);
        crow.Model.transform.rotation = Quaternion.Slerp(crow.Model.transform.rotation, target, fracComplete);

        float diff = crow.Model.transform.rotation.eulerAngles.z - target.eulerAngles.z;
        float degree = 1f;
        if (Mathf.Abs(diff) <= degree) //close enough to straight - reset damping
        {
            startZ = -1;
            hasTilted = false;
        }


    }

    public void TrailScale()
    {
        float scaleX = (speed - 10) / 30;
        if (scaleX < 0)
            scaleX = 0;
        Vector3 scaleChange = new Vector3(scaleX, scaleX, scaleX);
        LeftTrail.transform.localScale = scaleChange;
        RightTrail.transform.localScale = scaleChange;
    }
    private void OnDisable()
    {
        LeftTrail.SetActive(false);
        RightTrail.SetActive(false);
    }
    private void OnEnable()
    {
        ceiling.transform.position = new Vector3(ceiling.transform.position.x, transform.position.y + heightAboveLand, ceiling.transform.position.z);
        maxHeight = ceiling.transform.position.y;
        LeftTrail.SetActive(true);
        RightTrail.SetActive(true);
        speed = 10f;
        //Disabling all animator not related to flying(walking, idle)
        pcontroller.AnimationFlyingSuite();
    }
    public void SetFlightXY(float x, float y)
    {
        moveX = x;
        moveY = y;
    }
    public void SetBoost(bool b)
    {
        isBoosting = b;
    }

    public void SetBrake(bool b)
    {
        isBraking = b;
    }
}
