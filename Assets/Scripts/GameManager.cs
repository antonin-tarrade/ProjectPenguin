using Attacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Objet/Component
    private Player player;

    // Objet contenant toutes les statistiques, peut �tre customis� � volont� pour changer la difficult�(voir dossier GameData)
    public BattleData battleData;

    // Variable
    public bool isOver;
    public bool isPaused;
    public bool isShopOpen;

    public bool isStarted;
    public delegate void GameplayEvent();
    public GameplayEvent playerDeathEvent;
    public GameplayEvent playerRespawnEvent;
    public GameplayEvent pauseEvent;
    public GameplayEvent unpauseEvent;
    private UIManager UI;

    // ShopData
    public bool isFirstShop;

    // Singleton
    public static GameManager instance;

    private void Awake() {

        //Initialisation Component
        instance = this;
        isFirstShop = true;
    }
    
    void Start()
    {
        UI = UIManager.instance;
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    
    
    void Update()
    {
        // Game Over
        //if (player.health<=0) {
        //    isOver = true;            
        //}
        //else{
        //    isOver = false;
        //}
    }

    public void Play(){
        Time.timeScale = 1f;
        player.SetStats(battleData.playerStats);
        player.health = player.baseHealth;
        Player.iceShards = 0;

        UI.Switch(UI.gameMenu);
    }

    public void Pause(){
        Time.timeScale = 0f;
        UI.Switch(UI.pauseMenu);
        UI.pauseDefaultButton.Select();
        isPaused = true;
        pauseEvent?.Invoke();
    }

    public void ShopPause(){
        Time.timeScale = 0f;
        isShopOpen = true;
        isPaused = true;
    }

    public void Unpause(){
        isFirstShop = false;
        Time.timeScale = 1f;
        UI.Switch(UI.gameMenu);
        isPaused = false;
        unpauseEvent?.Invoke();
    }

    public void ShopUnpause(){
        Time.timeScale = 1f;
        isShopOpen = false;
        isPaused = false;
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
