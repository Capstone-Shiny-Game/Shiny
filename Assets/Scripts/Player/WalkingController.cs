using UnityEngine;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float TurningSpeed = 60;
    public float HeightOffset = 0.5f;

    void Start()
    {
        GetComponent<CameraController>().isWalking = true;
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        transform.eulerAngles = v;
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * TurningSpeed, 0);
        float displacement = Input.GetAxis("Vertical") * Time.deltaTime;
        displacement *= displacement >= 0 ? ForwardSpeed : BackwardsSpeed;
        transform.position += transform.forward * displacement;
        Vector3 pos = transform.position;
        pos.y = Terrain.activeTerrain.SampleHeight(pos) + HeightOffset;
        transform.position = pos;
    }
}