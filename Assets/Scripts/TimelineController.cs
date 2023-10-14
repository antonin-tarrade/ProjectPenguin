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
        Debug.Log("pause");
        playableDirector.Pause();
        StartCoroutine(CWaitForInput(key));
    }

    IEnumerator CWaitForInput(string key)
    {
        while (!Input.GetKeyDown(key))
        {
            yield return null;
            Debug.Log("waiting for input " + key);
        }
        Debug.Log("resume");
        playableDirector.Resume();
    }

}
