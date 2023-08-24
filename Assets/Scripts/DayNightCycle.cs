


using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{

    public static DayNightCycle instance;
    public enum Time { Day, Night }
    [HideInInspector]
    public Time cycleTime;

    // Event pour informer les autres lumières 
    public delegate void DayNightCycleEvent(Time time);
    public DayNightCycleEvent cycle;

    [Header("Parameters")]
    [SerializeField]
    private float cycleDuration;
    [SerializeField]
    private float minLightIntensity;
    [SerializeField]
    private float maxLightIntensity;
    [SerializeField]
    private float minIntensityDayTrigger;

    [SerializeField]
    private Light2D sun;
    private Vector3 sunPos;
    private Light2D globalLight;

    
    private float time;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        globalLight = GetComponent<Light2D>();
        sunPos = sun.gameObject.transform.localPosition;
        Day();
        time = minIntensityDayTrigger;
    }

    // Update is called once per frame
    void Update()
    {

        // Get time and set global lights settings
        time = (UnityEngine.Time.time) % (2* cycleDuration);
        float ratio = time / (2* cycleDuration);
        globalLight.intensity = ratio;
        float intensity = CustomTimeFunction(ratio);
        intensity = Mathf.Clamp(intensity, minLightIntensity, maxLightIntensity);
        globalLight.intensity = intensity;

        // Set possible changes 
        if (cycleTime == Time.Day && intensity < minIntensityDayTrigger) Night();
        else if (cycleTime == Time.Night && intensity >= minIntensityDayTrigger) Day();

        // Update position of the sun (pas poser de question)
        Camera camera = Camera.main;
        float sunOffset = 4 * ratio * camera.orthographicSize * camera.aspect;
        float offset = sunOffset / 2;
        sun.gameObject.transform.localPosition = sunPos + new Vector3(offset + sunOffset, 0, 0);

    }


    private void Day()
    {
        cycle?.Invoke(Time.Day);
        cycleTime = Time.Day;
    }

    private void Night()
    {
        cycle?.Invoke(Time.Night);
        cycleTime = Time.Night;
    }

    private float CustomTimeFunction(float x)
    {
        float y;
        if (0 <= x && x < 0.10) y = 10f * maxLightIntensity * x;
        else if (0.10 <= x && x < 0.4) y = maxLightIntensity;
        else if (0.4 <= x && x < 0.5) y = maxLightIntensity * (5 - 1 / 0.1f * x);
        else y = 0;
        return y;
    }

}

