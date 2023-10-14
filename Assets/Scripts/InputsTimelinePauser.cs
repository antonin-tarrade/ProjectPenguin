using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static ITimelineCustomPauser;

public interface ITimelineCustomPauser 
{

    public enum TimelineCustomPauserType { WaitForMultipleInputs}
    public void WaitForSignal(TimelineController controller);

    public void SendSignal();
}


public class InputsTimelinePauser : MonoBehaviour, ITimelineCustomPauser
{
    [SerializeField] private List<string> keys = new();
    [SerializeField] private List<string> buttons = new();
    [SerializeField] private List<string> triggers = new();
    private TimelineController pausedTimeline;
    private PlayableDirector playable;


    private void Start()
    {
        triggers = new List<string>();
        triggers.AddRange(keys);
        triggers.AddRange(buttons);
    }
    public void SendSignal()
    {
        playable.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void WaitForSignal(TimelineController controller)
    {
        playable = controller.gameObject.GetComponent<PlayableDirector>();
        playable.playableGraph.GetRootPlayable(0).SetSpeed(0);
        pausedTimeline = controller;
    }

    private void SendInput(string input)
    {
        triggers.Remove(input);
        if (triggers.Count == 0) SendSignal();
    }

    private void Update()
    {
        Debug.Log(Input.GetButton("Fire1"));
        List<string> toRemoveKeys = new();
        List<string> toRemoveButtons = new();
        foreach (string key in keys)
        {   
            if (Input.GetKey(key))
            {
                toRemoveKeys.Add(key);
            }
        }
        foreach (string button in buttons)
        {
            if (Input.GetButton(button))
            {
                toRemoveButtons.Add(button);
            }
        }
        foreach (string key in toRemoveKeys)
        {
            SendInput(key);
            keys.Remove(key);
        }
        foreach (string button in toRemoveButtons)
        {
            SendInput(button);
            buttons.Remove(button);
        }
    }
}


