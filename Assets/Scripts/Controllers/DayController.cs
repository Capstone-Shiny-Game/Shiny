using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayController : MonoBehaviour
{
    [Range(0, 24)] public float TimeOfDay;
    public string CurrentDay;
    [SerializeField] private Light sun;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private float LengthOfDay;
    private string[] DaysOfWeek = {"Sunday","Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"};
    private int DayIndex = 0;


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

        float timeElapsed = 0;
        while (timeElapsed < LengthOfDay)
        {
            TimeOfDay = Mathf.Lerp(0, 24, timeElapsed/LengthOfDay);
            timeElapsed += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay/24f);
            yield return null;
        }
        DayIndex++;
        if(DayIndex > 6)
        {
            DayIndex = 0;
        }
        CurrentDay = DaysOfWeek[DayIndex];
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

