using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    public List<GameObject> rings;
    public float timeInterval;
    public List<string> activeDaysOfWeek;
    public TextMeshProUGUI timerText;

    private LinkedList<GameObject> ringOrder;
    private GameObject currRing;
    private Coroutine startTimePeriod;
    private bool completed;

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

        // *comment out for testing!
        //gameObject.SetActive(false);

        completed = false;
    }

    // added to work with hot air balloons
    private void OnTriggerEnter(Collider other)
    {
        if (!completed)
        {
            ringOrder.First.Value.SetActive(true);
            startTimePeriod = StartCoroutine(StartTimePeriod());
        }
    }

    private void OnEnable()
    {
        TimeTrialRing.OnRingCollision += UpdateTimeTrial;
        DayController.OnNewDayEvent += AppearOnCorrectDay;
    }

    private void OnDisable()
    {
        TimeTrialRing.OnRingCollision -= UpdateTimeTrial;
        StopAllCoroutines();
    }

    private void AppearOnCorrectDay(string day)
    {
        if (activeDaysOfWeek.Contains(day))
        {
            // the trial has previously been completed
            if (currRing == null)
            {
                completed = false;
                currRing = ringOrder.First.Value;
            }
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
            //StartCoroutine(DeactivateDelay(ring));
            currRing = ringOrder.Find(ring).Next?.Value;
            if (currRing == null)
            {
                // win state
                completed = true;
                StopCoroutine(startTimePeriod);
                StartCoroutine(DisplayWinText());
                // cleanup
                StartCoroutine(DeactivateAllRingsWithDelay());
                //Destroy(gameObject);
                //foreach (GameObject r in rings)
                //{
                //    Destroy(r);
                //}
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
        timerText.gameObject.SetActive(true);

        float timeElapsed = 0;
        while (timeElapsed < timeInterval)
        {
            timeElapsed += Time.deltaTime;

            // update the displayed time remaining
            float remainingTime = timeInterval - timeElapsed;
            float seconds = Mathf.FloorToInt(remainingTime % 60);
            float milliSeconds = (remainingTime % 1) * 1000;
            timerText.text = string.Format("{0:00}:{1:000}", seconds, milliSeconds);

            yield return new WaitForEndOfFrame();
        }

        // gets here if player does not make it to the current ring on time,
        // reset the time trial
        currRing.SetActive(false);
        ringOrder.Find(currRing).Next?.Value.SetActive(false);
        currRing = ringOrder.First.Value;
        //currRing.SetActive(true);
        currRing.SetActive(false);

        timerText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayWinText()
    {
        timerText.text = "You beat the time trial!";
        yield return new WaitForSeconds(5);
        timerText.text = string.Empty;
        timerText.gameObject.SetActive(false);
    }

    private IEnumerator DeactivateAllRingsWithDelay()
    {
        yield return new WaitForSeconds(0.5f);

        LinkedListNode<GameObject> r = ringOrder.Last;
        while (r != null)
        {
            LinkedListNode<GameObject> rPrev = r.Previous;
            r.Value.SetActive(false);
            r = rPrev;
            yield return new WaitForSeconds(0.5f);
        }
    }

    //private IEnumerator DeactivateDelay(GameObject ring)
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    ring?.SetActive(false);
    //}
}
