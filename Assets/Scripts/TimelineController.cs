using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{


    PlayableDirector playableDirector;


    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    public void Test(KeyCode key)
    {

    }

    public void WaitForInput(string key)
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(0);
        StartCoroutine(CWaitForInput(key));
    }

    IEnumerator CWaitForInput(string key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
        }
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }


}
