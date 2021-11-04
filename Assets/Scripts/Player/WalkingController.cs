using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WalkingController : MonoBehaviour
{
    public float ForwardSpeed = 8;
    public float BackwardsSpeed = 4;
    public float TurningSpeed = 60;
    public float HeightOffset = 0.5f;

    private GroundDetector ground;
    private bool useTerrain;

    void Start()
    {
        Vector3 v = transform.eulerAngles;
        v.x = 0;
        transform.eulerAngles = v;

        // TODO (Ella) *hisses at these sins*
        useTerrain = SceneManager.GetActiveScene().name == "WalkingTest";
        if (!useTerrain)
            ground = GetComponent<GroundDetector>() ?? gameObject.AddComponent<GroundDetector>();
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * Time.deltaTime * TurningSpeed, 0);
        float displacement = Input.GetAxis("Vertical") * Time.deltaTime;
        displacement *= displacement >= 0 ? ForwardSpeed : BackwardsSpeed;
        transform.position += transform.forward * displacement;
        Vector3 pos = transform.position;
        // TODO (Ella): will we ever have more than one terrain
        if (useTerrain)
            pos.y = Terrain.activeTerrains.Select(t => t.SampleHeight(pos)).Max() + HeightOffset;
        else if (ground.FindGround() is Vector3 groundPos)
            pos.y = groundPos.y + HeightOffset;
        transform.position = pos;
    }
}