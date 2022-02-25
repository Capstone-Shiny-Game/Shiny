using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayController : MonoBehaviour
{
    [Range(0, 24)] public float TimeOfDay;
    public float TEMPDayStartTime;
    public string CurrentDay;
    [SerializeField] private Light sun;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private float LengthOfDay;
    private string[] DaysOfWeek = {"Sunday","Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
    private int DayIndex = 0;
    private const float morning = 6.0f;
    private const float midDay = 12.0f;
    private const float evening = 18.0f;

    public delegate void DayStart();
    public static event DayStart OnDayStartEvent;
    public delegate void Morning();
    public static event Morning OnMorningEvent;
    public delegate void MidDay();
    public static event MidDay OnMidDayEvent;
    public delegate void Evening();
    public static event Evening OnEveningEvent;



    private void Start()
    {
        if(Application.isPlaying)
        {
            LengthOfDay *= 60;
            CurrentDay = DaysOfWeek[DayIndex];
            StartCoroutine(Lerp());
        }
    }

    private void Update()
    {
        if(!Application.isPlaying)
        {
            UpdateLighting(TimeOfDay/24f);
        }
    }

    private IEnumerator Lerp()
    {
        bool hasInvoked = false;
        float timeElapsed = 0;
        while (timeElapsed < LengthOfDay)
        {
            TimeOfDay = Mathf.Lerp(TEMPDayStartTime, 24, timeElapsed/LengthOfDay);
            timeElapsed += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay/24f);
            switch(Mathf.Floor(TimeOfDay))
            {
                case 0:
                    if(!hasInvoked)
                        OnDayStartEvent?.Invoke();
                        hasInvoked = true;
                    break;

                case morning:
                    if(!hasInvoked)
                        OnMorningEvent?.Invoke();
                        hasInvoked = true;
                    break;

                case midDay:
                    if(!hasInvoked)
                        OnMidDayEvent?.Invoke();
                        hasInvoked = true;
                    break;

                case evening:
                    if(!hasInvoked)
                        OnEveningEvent?.Invoke();
                        hasInvoked = true;
                    break;
                
                default:
                    hasInvoked = false;
                    break;
            }
            yield return null;
        }
        DayIndex++;
        if(DayIndex > 6)
        {
            DayIndex = 0;
        }
        CurrentDay = DaysOfWeek[DayIndex];
        TEMPDayStartTime = 0;
        StartCoroutine(Lerp());
    }

    private void UpdateLighting(float timePercent)
    {
        if(Preset == null)
            return;

        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        RenderSettings.fogDensity = Preset.FogIntensity.Evaluate(timePercent);

        if(sun != null)
        {
            sun.color = Preset.DirectionalColor.Evaluate(timePercent);
            sun.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 190f, 0));
        }
    }

    private void OnValidate()
    {
        if(sun != null)
            return;
        
        if(RenderSettings.sun != null)
        {
            sun = RenderSettings.sun;
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    sun = light;
                    return;
                }
            }
        }
    }
}

