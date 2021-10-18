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
    float tiltAngle = 60.0f;
    // Start is called before the first frame update

    public Transform start;
    public Transform end;

    private Vector3 startOffset;
    private Vector3 endOffset;
    // Time to move from sunrise to sunset position, in seconds.
    public float journeyTime = 1.0f;

    // The time at which the animation started.
    private float startTime =-1;
    private bool doUTurn = false;


    void Start()
    {
        startOffset = start.position - transform.position;
        endOffset = end.position - transform.position;
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
            if (startTime <0)
            {
                Debug.Log("setting start time");
                startTime = Time.time;
                transform.position = start.position;
            }
            Somersault(startTime);
        }

        FlyTwoAxis();   
    }

    private void FlyTwoAxis()
    {
        //currForwardSpd = Input.GetAxisRaw("Vertical") * forwardSpd;
        ////currStrafeSpd = Input.GetAxisRaw("Horizontal") * strafeSpd;
        //currHoverSpd = Input.GetAxisRaw("Elevate") * hoverSpd;
        //transform.position += (transform.forward * currForwardSpd * Time.deltaTime)+ (transform.up * currHoverSpd*Time.deltaTime);
        ////transform.position += (transform.up * currHoverSpd * Time.deltaTime);
        //float tiltAroundY = Input.GetAxis("Horizontal") * tiltAngle;
        ////float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;

        ////// Rotate the cube by converting the angles into a quaternion.
        //Quaternion target = Quaternion.Euler(0, tiltAroundY, 0);

        ////// Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
       
  

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


        if (fracComplete >= 1f)
        {
            ResetSomersault();
        }
    }
    private void ResetSomersault()
    {
        Debug.Log("restarting slerp");
        //Rotation slerp has finished. Do something else now.
        doUTurn = false;
        startTime = -1;

        start.position = transform.position + startOffset;
        end.position = transform.position +  endOffset;
        //startOffset = start.position - transform.position;
        //endOffset = end.position - transform.position;
    }
}
