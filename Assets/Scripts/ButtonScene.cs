using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Attacks;

public class ButtonScene : MonoBehaviour
{
    
    
    public void RetourMenu()
    {
    	SceneManager.LoadScene("Menu");
    }
    
    public void QuitGame()
    {
    	Application.Quit();
    }
}
