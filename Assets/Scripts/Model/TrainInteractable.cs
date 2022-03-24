using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainInteractable : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private Transform offset;
    public int numCoals = 10;
    public float delayMult = 0.1f;

    private const float noiseRange = 50f;
    private const float forceMin = 500;
    private const float forceMax = 1500;
    private const float delayMax = 0.25f;
    private const float torqueHalfAbs = 50f;

    private void Start()
    {

    }

    public void OnClick()
    {
        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);

        
        for (int i = 0; i < numCoals; i++)
        {
            StartCoroutine(SpawnCoal(i * delayMult + Random.Range(0f, delayMax)));
        }
    }

    private IEnumerator SpawnCoal(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);
        GameObject coal = Instantiate(objectToSpawn, offset.position, transform.rotation);
        coal.GetComponentInChildren<Rigidbody>().AddForce(new Vector3(Random.Range(-noiseRange, noiseRange), Random.Range(forceMin, forceMax), Random.Range(-noiseRange, noiseRange)));
        coal.GetComponentInChildren<Rigidbody>().AddTorque(new Vector3(Random.Range(-torqueHalfAbs, torqueHalfAbs), Random.Range(-torqueHalfAbs, torqueHalfAbs), Random.Range(-torqueHalfAbs, torqueHalfAbs)));
    }
}
