using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLancerPartie : MonoBehaviour
{
	
    public void LancerPartie()
    {
    	SceneManager.LoadScene("SampleScene");
    	
    }
    
    // après avoir choisi la difficulté
    public void DemarrerPartie()
    {
    	GameManager.instance.Play();
        GameManager.instance.isStarted = true;
    }
}
