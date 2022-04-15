using UnityEngine;

[UnityEngine.RequireComponent(typeof(AkGameObj))]
public class TriggerPlayer : MonoBehaviour
{

    public AK.Wwise.Event EnterEvent;
    public AK.Wwise.Event ExitEvent;

    private bool night = false;

    void Start()
    {
        DayController.OnEveningEvent += () => night = true;
        DayController.OnMorningEvent += () => night = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(EnterEvent == null || !other.CompareTag("Player") || night)
        {
            return;
        }

        EnterEvent.Post(gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if(ExitEvent == null || !other.CompareTag("Player") || night)
        {
            return;
        }

        ExitEvent.Post(gameObject);
    }
}
