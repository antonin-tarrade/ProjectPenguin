using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(Light2D))]
public class DayNightCycleTrigger : MonoBehaviour
{
    [SerializeField]
    private float transitionSpeed;
    [SerializeField]
    private float maxIntensity;
    [SerializeField]
    private float minIntensity;

    private new Light2D light;

    private void Start()
    {
        light = GetComponent<Light2D>();
        DayNightCycle.instance.cycle += Set;

        Set(DayNightCycle.instance.cycleTime);

        //Pour scaler
        transitionSpeed /= 100;
    }


    // Change l'état
    public void Set(DayNightCycle.Time time)
    {
        StopAllCoroutines();
        if (time == DayNightCycle.Time.Day)
        {
            StartCoroutine(TurnOff(transitionSpeed));
        }
        else
        {
            StartCoroutine(TurnOn(transitionSpeed));
        }
    }


    // Allume/éteint progressivement la lumière
    IEnumerator TurnOn(float transitionSpeed)
    {
        while (light.intensity < maxIntensity) {
            light.intensity += transitionSpeed;
            yield return null;
        }
    }

    IEnumerator TurnOff(float transitionSpeed)
    {
        while (light.intensity > minIntensity)
        {
            light.intensity -= transitionSpeed;
            yield return null;
        }
    }

    private void OnDestroy()
    {
        DayNightCycle.instance.cycle -= Set;
    }
}