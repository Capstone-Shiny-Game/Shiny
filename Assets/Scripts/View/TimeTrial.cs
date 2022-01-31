using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    public List<GameObject> rings;

    private LinkedList<GameObject> ringOrder;
    private GameObject currRing;

    void Start()
    {
        ringOrder = new LinkedList<GameObject>();
        // add each ring to the linked list and only show the first one
        foreach (GameObject ring in rings)
        {
            ringOrder.AddLast(ring);
            ring.SetActive(false);
        }
        ringOrder.First.Value.SetActive(true);
        currRing = ringOrder.First.Value;
    }

    private void OnEnable()
    {
        TimeTrialRing.OnRingCollision += UpdateTimeTrial;
    }

    private void OnDisable()
    {
        TimeTrialRing.OnRingCollision -= UpdateTimeTrial;
    }

    private void UpdateTimeTrial(GameObject ring)
    {

    }
}
