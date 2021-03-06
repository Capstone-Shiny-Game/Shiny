using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeTrial : MonoBehaviour
{
    public string Name;

    public List<GameObject> rings;
    public float timeInterval;
    public List<string> activeDaysOfWeek;
    public TextMeshProUGUI timerText;

    private LinkedList<GameObject> ringOrder;
    private GameObject currRing;
    private Coroutine startTimePeriod;
    private bool completed;

    public delegate void Complete();
    public static event Complete OnTimeTrialCompleteEvent;

    void Start()
    {
        ringOrder = new LinkedList<GameObject>();
        // add each ring to the linked list
        foreach (GameObject ring in rings)
        {
            ringOrder.AddLast(ring);
            ring.SetActive(false);
        }
        currRing = ringOrder.First.Value;

        AppearOnCorrectDay("Sunday");
        completed = false;
    }

    // added to work with hot air balloons
    private void OnTriggerEnter(Collider other)
    {
        if (!completed)
        {
            QuestManager.StartQuest(Name, null, null);
            ActivateRing(ringOrder.First.Value);
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
            currRing = ringOrder.Find(ring).Next?.Value;
            if (currRing == null)
            {
                // win state
                completed = true;
                QuestManager.CompleteQuest(Name);
                StopCoroutine(startTimePeriod);
                StartCoroutine(DisplayWinText());
                // cleanup
                StartCoroutine(DeactivateRingsWithDelay());
            }
            else
            {
                // activate next two rings
                if (startTimePeriod != null)
                {
                    StopCoroutine(startTimePeriod);
                }
                ActivateRing(currRing);
                ActivateRing(ringOrder.Find(currRing).Next?.Value);
                startTimePeriod = StartCoroutine(StartTimePeriod());
            }
        }
    }

    private void ActivateRing(GameObject ring)
    {
        if (ring != null && !ring.activeSelf)
        {
            ring.SetActive(true);
            StartCoroutine(PopInRing(ring));
        }
    }

    private IEnumerator StartTimePeriod()
    {
        if (!timerText.gameObject.activeSelf)
        {
            timerText.gameObject.SetActive(true);
        }

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
        StartCoroutine(DeactivateRingsWithDelay());
        currRing = ringOrder.First.Value;

        timerText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayWinText()
    {
        OnTimeTrialCompleteEvent?.Invoke();
        timerText.text = $"You beat {QuestManager.ExpandName(Name)}!";
        yield return new WaitForSeconds(5);
        timerText.text = string.Empty;
        timerText.gameObject.SetActive(false);
    }

    private IEnumerator DeactivateRingsWithDelay()
    {
        if (completed)
            yield return new WaitForSeconds(0.5f);

        LinkedListNode<GameObject> r = ringOrder.Last;
        while (r != null)
        {
            LinkedListNode<GameObject> rPrev = r.Previous;
            // don't wait on rings that were never active
            if (completed || r.Value.activeInHierarchy)
            {
                r.Value.SetActive(false);
            }
            r = rPrev;
        }
    }

    private IEnumerator PopInRing(GameObject ring)
    {
        Vector3 endScale = ring.transform.localScale;
        ring.transform.localScale *= 0.1f;

        while (ring.transform.localScale.x < endScale.x)
        {
            ring.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            yield return new WaitForEndOfFrame();
        }
    }
}
