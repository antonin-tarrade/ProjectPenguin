using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Objet/Component
    public GameObject player;
    private Player playerSystem;

    public GameObject UI;
    private UIManager UIManager;

    // Variable
    public bool isOver;

    private void Awake() {
        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
        UIManager = UI.GetComponent<UIManager>();
    }
    
    void Update()
    {
        // Game Over
        if (playerSystem.health<=0) {
            isOver = true;            
        }
        else{
            isOver = false;
        }
    }

    public void Play(){
        Time.timeScale = 1f;

        // playerSystem.health = playerSystem.baseHealth;
        playerSystem.iceShards = 0;

        UIManager.Switch(UIManager.gameMenu);
    }

    public void Pause(){
        Time.timeScale = 0f;
        UIManager.Switch(UIManager.pauseMenu);
        UIManager.isPaused = !(UIManager.isPaused);
    }

    public void Unpause(){
        Time.timeScale = 1f;
        UIManager.Switch(UIManager.gameMenu);
        UIManager.isPaused = !(UIManager.isPaused);
    }
}
