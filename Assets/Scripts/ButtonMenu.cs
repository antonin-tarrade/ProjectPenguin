using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{

    void Start()
    {
        if (gameObject.name == "Return") gameObject.GetComponent<Button>().Select();
    }
    
    public void GoToRule()
    {
    	SceneManager.LoadScene("Rules");
    }
    
    public void GoBackToMenu()
    {
    	SceneManager.LoadScene("Menu");
    }
}
