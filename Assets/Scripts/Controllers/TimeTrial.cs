using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    public List<GameObject> rings;
    public float timeInterval;

    private LinkedList<GameObject> ringOrder;
    private GameObject currRing;
    private Coroutine startTimePeriod;

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
        StopAllCoroutines();
    }

    private void UpdateTimeTrial(GameObject ring)
    {
        if (ring == currRing)
        {
            currRing.SetActive(false);
            currRing = ringOrder.Find(ring).Next?.Value;
            if (currRing == null)
            {
                // win state
                StopCoroutine(startTimePeriod);
                Debug.Log("You beat the time trial!");
                // cleanup
                Destroy(gameObject);
            }
            else
            {
                // activate next ring
                if (startTimePeriod != null)
                {
                    StopCoroutine(startTimePeriod);
                }
                currRing.SetActive(true);
                startTimePeriod = StartCoroutine(StartTimePeriod());
            }
        }
    }

    private IEnumerator StartTimePeriod()
    {
        yield return new WaitForSeconds(timeInterval);

        // gets here if player does not make it to the current ring on time,
        // reset the time trial
        currRing.SetActive(false);
        currRing = ringOrder.First.Value;
        currRing.SetActive(true);
    }
}
