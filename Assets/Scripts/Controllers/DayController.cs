using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DayController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private LightingPreset Preset;
    [Range(0, 24)] public float TimeOfDay;

    private void Update()
    {
        if(Preset == null)
            return;

        if(Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay/24f);
        }
        else
        {
            UpdateLighting(TimeOfDay/24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        RenderSettings.fogDensity = Preset.FogIntensity.Evaluate(timePercent);

        if(sun != null)
        {
            sun.color = Preset.DirectionalColor.Evaluate(timePercent);
            sun.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
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

