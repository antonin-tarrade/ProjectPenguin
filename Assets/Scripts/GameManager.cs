using Attacks;
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

    // Objet contenant toutes les statistiques, peut �tre customis� � volont� pour changer la difficult�(voir dossier GameData)
    public BattleData battleData;

    // Variable
    public bool isOver;
    public bool isPaused;
    public bool isStarted;
    public delegate void GameplayEvent();
    public GameplayEvent playerDeathEvent;
    public GameplayEvent playerRespawnEvent;
    public GameplayEvent pauseEvent;
    public GameplayEvent unpauseEvent;


    public static GameManager instance;



    private void Awake() {
        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
        UIManager = UI.GetComponent<UIManager>();
        isStarted = false;
        instance = this;
    }
    
    void Start()
    {
    	//Pause();
        //Unpause();
    }
    
    
    void Update()
    {
        // Game Over
        //if (playerSystem.health<=0) {
        //    isOver = true;            
        //}
        //else{
        //    isOver = false;
        //}
    }

    public void Play(){
        Time.timeScale = 1f;

        playerSystem.SetStats(battleData.playerStats);
        playerSystem.health = playerSystem.baseHealth;
        playerSystem.iceShards = 0;

        UIManager.Switch(UIManager.gameMenu);
        isStarted = true;
    }

    public void Pause(){
        Time.timeScale = 0f;
        UIManager.Switch(UIManager.pauseMenu);
        UIManager.pauseDefaultButton.Select();
        UIManager.isPaused = !(UIManager.isPaused);
        this.isPaused = true;
        pauseEvent?.Invoke();
    }

    public void ShopPause(){
        Time.timeScale = 0f;
        UIManager.Switch(UIManager.shopMenu);
        UIManager.isPaused = !(UIManager.isPaused);
        this.isPaused = true;
    }

    public void Unpause(){
        Time.timeScale = 1f;
        UIManager.Switch(UIManager.gameMenu);
        UIManager.isPaused = !(UIManager.isPaused);
        this.isPaused = false;
        unpauseEvent?.Invoke();
    }

    public void PlayerDeath()
    {
        isOver = true;
        playerDeathEvent?.Invoke();
    }

    public void PlayerRespawn()
    {
        isOver = false;
        playerRespawnEvent?.Invoke();
    }
    
    

    
}
