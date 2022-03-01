using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    public List<GameObject> rings;
    public float timeInterval;
    public List<string> activeDaysOfWeek;

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
        currRing = ringOrder.First.Value;
        //ringOrder.First.Value.SetActive(true);
        gameObject.SetActive(false);
    }

    // added to work with hot air balloons
    private void OnTriggerEnter(Collider other)
    {
        ringOrder.First.Value.SetActive(true);
        startTimePeriod = StartCoroutine(StartTimePeriod());
    }

    private void OnEnable()
    {
        TimeTrialRing.OnRingCollision += UpdateTimeTrial;
        DayController.OnNewDayEvent += AppearOnCorrectDay;
    }

    private void OnDisable()
    {
        TimeTrialRing.OnRingCollision -= UpdateTimeTrial;
        DayController.OnNewDayEvent -= AppearOnCorrectDay;
        StopAllCoroutines();
    }

    private void AppearOnCorrectDay(string day)
    {
        if (activeDaysOfWeek.Contains(day))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
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
                //Destroy(gameObject);
                foreach (GameObject r in rings)
                {
                    Destroy(r);
                }
            }
            else
            {
                // activate next two rings
                if (startTimePeriod != null)
                {
                    StopCoroutine(startTimePeriod);
                }
                currRing.SetActive(true);
                ringOrder.Find(currRing).Next?.Value.SetActive(true);
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
        ringOrder.Find(currRing).Next?.Value.SetActive(false);
        currRing = ringOrder.First.Value;
        //currRing.SetActive(true);
        currRing.SetActive(false);
    }
}
