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
    public float forwardSpd = 25f, strafeSpd = 7.5f, hoverSpd = 5f;
    private float currForwardSpd, currStrafeSpd, currHoverSpd;
    float smooth = 5.0f;
    float tiltAngle = 60.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FlyTwoAxis();   
    }

    private void FlyTwoAxis()
    {
        currForwardSpd = Input.GetAxisRaw("Vertical") * forwardSpd;
        //currStrafeSpd = Input.GetAxisRaw("Horizontal") * strafeSpd;

        transform.position += (transform.forward * currForwardSpd * Time.deltaTime);

        float tiltAroundY = Input.GetAxis("Horizontal") * tiltAngle;

        // Rotate the cube by converting the angles into a quaternion.
        Quaternion target = Quaternion.Euler(0, tiltAroundY, 90);

        // Dampen towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
