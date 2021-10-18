/**
 * 
 * Controls
*When there is no input during flight mode the crow will spread its wings, slow down speed and glide forward. We will manipulate the FOV to infer speed.
*When going fast, focus out a bit and add speed lines
*When gliding, zoom in, no speed lines
*
*There are currently 2 possible control schemes for flight TBD. We plan to test these two and then see which one is better and more natural:
*
*Control Scheme 1: Flying only in X,Y axis, control Z axis with a separate button.
*W/Joystick forward: speed up and go forward
*A/Leftward joystick : turn/ dip left
*D/Rightward joystick : turn/ dip right
*S/Joystick backwards: Somersault uturn
*Q/Left bumper: Fly upwards 
*E/Right bumper: Fly downwards
*Mouse/right joystick: Camera control
 * 
 * 
 * 
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightController : MonoBehaviour
{
    public float forwardSpd = 15f, strafeSpd = 7.5f, hoverSpd = 5f;
    private float currForwardSpd, currStrafeSpd, currHoverSpd;
    float smooth = 5.0f;
    float tiltAngle = .60f;
    public Camera cam;
    // Start is called before the first frame update

    public Transform start;
    public Transform end;

    // Time to move from sunrise to sunset position, in seconds.
    public float journeyTime = 1.0f;

    // The time at which the animation started.
    private float startTime = -1;
    private bool doUTurn = false;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            doUTurn = true;
        }
        if (doUTurn)
        {
            if (startTime < 0)
            {
                SetupSomersault();
            }
            Somersault(startTime);
        }

        FlyTwoAxis();
    }
    /// <summary>
    /// Prep the start and end positions of the Slerp. The start is the player's position, the end is the midpoint between the player and the camera.
    /// </summary>
    private void SetupSomersault()
    {
        Debug.Log("setting start time");
        startTime = Time.time;
        start.position = transform.position;
        float newX = transform.position.x + (cam.transform.position.x - transform.position.x) / 2;
        float newY = transform.position.y + (cam.transform.position.y - transform.position.y) / 2 + 10f;
        float newZ = transform.position.z + (cam.transform.position.z - transform.position.z) / 2;

        end.position = new Vector3(newX, newY, newZ);
    }
    private void FlyTwoAxis()
    {
        currForwardSpd = Input.GetAxisRaw("Vertical") * forwardSpd;
        float tiltAroundY = Input.GetAxis("Horizontal") * tiltAngle;
        currHoverSpd = Input.GetAxisRaw("Elevate") * hoverSpd;

        transform.position += (transform.forward * currForwardSpd * Time.deltaTime) + (transform.up * currHoverSpd * Time.deltaTime);
        transform.Rotate(0, tiltAroundY, 0);
    }
    private void Somersault(float startTime)
    {
        // The center of the arc
        Vector3 center = (start.position + end.position) * 0.5F;

        // move the center a bit downwards to make the arc vertical
        center -= new Vector3(0, 1, 0);

        // Interpolate over the arc relative to center
        Vector3 riseRelCenter = start.position - center;
        Vector3 setRelCenter = end.position - center;

        // The fraction of the animation that has happened so far is
        // equal to the elapsed time divided by the desired time for
        // the total journey.
        float fracComplete = (Time.time - startTime) / journeyTime;

        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;
        //cam.transform.RotateAround(transform.position, new Vector3(0, 1, 0), 180f * Time.deltaTime);
        StartCoroutine("StartCameraFlip");
        if (fracComplete >= 1f)
        {
            ResetSomersault();
        }
    }
    private IEnumerator StartCameraFlip()
    {
        yield return new WaitForSeconds(0.5f);
        transform.RotateAround(transform.position, new Vector3(0, 1, 0), 180f * Time.deltaTime);

    }
    private void ResetSomersault()
    {
        Debug.Log("restarting slerp");
        //Rotation slerp has finished. Do something else now.
        doUTurn = false;
        startTime = -1;



        //TO DO: rotate camera to follow after uturn
        //cam.transform.position = new Vector3(cam.transform.position.x,cam.transform.position.y,-cam.transform.position.z);
        //transform.Rotate(0, 180, 0);
    }
}
