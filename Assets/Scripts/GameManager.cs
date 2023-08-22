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

    // Objet contenant toutes les statistiques, peut être customisé à volonté pour changer la difficulté(voir dossier GameData)
    public BattleData battleData;

    // Variable
    public bool isOver;
    public delegate void GameplayEvent();
    public GameplayEvent playerDeathEvent;
    public GameplayEvent playerRespawnEvent;


    public static GameManager instance;

    private void Awake() {
        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
        UIManager = UI.GetComponent<UIManager>();

        instance = this;
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
    }

    public void Pause(){
        Time.timeScale = 0f;
        UIManager.Switch(UIManager.pauseMenu);
        UIManager.isPaused = !(UIManager.isPaused);
    }

    public void ShopPause(){
        Time.timeScale = 0f;
        UIManager.Switch(UIManager.shopMenu);
        UIManager.isPaused = !(UIManager.isPaused);
    }

    public void Unpause(){
        Time.timeScale = 1f;
        UIManager.Switch(UIManager.gameMenu);
        UIManager.isPaused = !(UIManager.isPaused);
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
