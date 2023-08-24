using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// Pour les processus non monobehaviour qui doivent tourner en arrière plan
// Voir exemple d'utilisation pour le slow shot dans le scrip Attacks
public class BackgroundUpdater : MonoBehaviour
{
    public delegate void TimeFunc();

    public class Timer
    {
        private float time;
        private float timer;

        public TimeFunc timeFunc;

        public void TimerUpdate(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= time)
            {
                timeFunc.Invoke();
                BackgroundUpdater.instance.Remove(this);
            }
        }

        public Timer(float time, TimeFunc timeFunc)
        {
            this.time = time;
            timer = 0;
            this.timeFunc = timeFunc;
            BackgroundUpdater.instance.Add(this);
        }
    }

    public static BackgroundUpdater instance;

    private List<Timer> timers = new();

    private List<Timer> timersToAdd = new();

    private List<Timer> timersToRemove = new();

    public void Add(Timer timer) { timersToAdd.Add(timer); }

    public void Remove(Timer timer) { timersToRemove.Add(timer); }

    private void Awake()
    {
        instance = this;
    }


    // Update is called once per frame
    void Update()
    {
        if (timersToAdd.Count > 0) { 
            timers.AddRange(timersToAdd);
            timersToAdd.Clear();
        }

        if (timersToRemove.Count > 0) { 
            timers = timers.Except<Timer>(timersToRemove).ToList();
        }

        foreach (Timer timer in timers)
        {
            timer.TimerUpdate(Time.deltaTime);
        }


    }
}

