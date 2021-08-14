using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightningManager : MonoBehaviour
{
    public event Action CallDayEvent;

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset lightingPreset;
    [SerializeField] private float timeOFDay;

    private int days = 0;

    private void Awake()
    {
        CallDayEvent += DayCount;
    }

    private void DayCount()
    {
        days += (int)(timeOFDay / (24f*60f));
        Debug.Log("Day " + days);
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            timeOFDay += Time.deltaTime;
            timeOFDay %= 24f*60f;
            UpdateLighting(timeOFDay/(24f * 60f));
        }
        else
        {
            UpdateLighting(timeOFDay / (24f * 60f));
        }

        if(Mathf.Round(timeOFDay % (24f*60f)) == 12f*60f)
        {
            Debug.Log("ShouldCall");
            CallDayEvent?.Invoke();
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = lightingPreset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = lightingPreset.FogColor.Evaluate(timePercent);
        directionalLight.color = lightingPreset.DirectionalColor.Evaluate(timePercent);
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0f));
    }
}
