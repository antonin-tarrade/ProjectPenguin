using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    
    [SerializeField] GameObject canvas;
    
    public void Activer()
    {
    	canvas.SetActive(true);
    }
    
    public void Retour()
    {
    	canvas.SetActive(false);
    }
}
