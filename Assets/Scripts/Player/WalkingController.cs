using UnityEngine;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8f;
    public float BackwardsSpeed = 4f;
    public float TurningSpeed = 60f;

    void Start()
    {
        GetComponent<CameraController>().isWalking = true;
        Vector3 v = transform.eulerAngles;
        v.x = 0f;
        transform.eulerAngles = v;
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * TurningSpeed, 0);
        float displacement = Input.GetAxis("Vertical") * Time.deltaTime;
        displacement *= displacement >= 0 ? ForwardSpeed : BackwardsSpeed;
        transform.position += transform.forward * displacement;
    }
}