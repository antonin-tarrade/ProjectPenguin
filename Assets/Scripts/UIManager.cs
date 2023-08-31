using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using JetBrains.Annotations;

public class UIManager : MonoBehaviour
{
    // Objet/Component
    // GameManager
    public GameObject GameManager;
    private GameManager gameManager;
    // Player
    public GameObject player;
    private Player playerSystem;
    // enemySpawner
    public GameObject enemySpawner;
    private EnemySpawner enemySpawnerSystem;

    // Menu
    public GameObject mainMenu;
    public GameObject rulesMenu;
    public GameObject creditMenu;
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject overMenu;
    public GameObject shopMenu;

    
    // UIs
    public GameObject UIHealth;
    private Health UIHealthSystem;
    public GameObject UIShards;
    public GameObject UIWave;

    // Variables
    public GameObject menuActif;

    public bool isPaused;

    private void Awake() {
        Initialisation();

        //Initialisation Component
        playerSystem = player.GetComponent<Player>();
        gameManager = GameManager.GetComponent<GameManager>();
        UIHealthSystem = UIHealth.GetComponent<Health>();
        UIHealthSystem.InitHealthUI(playerSystem.baseHealth);
        enemySpawnerSystem = enemySpawner.GetComponent<EnemySpawner>();
    }

    public void Initialisation() {

        Time.timeScale = 0f;
        
        menuActif = mainMenu;
        mainMenu.SetActive(true);
        rulesMenu.SetActive(false);
        creditMenu.SetActive(false);
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        overMenu.SetActive(false);
        shopMenu.SetActive(false);

    }

    private void Start()
    {
        gameManager.playerDeathEvent += onPlayerDeath;
        gameManager.playerRespawnEvent += onPlayerRespawn;
    }

    private void Update() {

        // Menu Over
        if (menuActif == overMenu)
        {
            if (Input.GetKeyDown(KeyCode.R)) gameManager.PlayerRespawn();

        }

        // Menu Pause
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                gameManager.Pause();
            }
            else {
                gameManager.Unpause();
            }
        }
        if (isPaused) return;

        // Menu Game
       
         
        // Health (UI)
        UIHealthSystem.UpdateHealthUI(playerSystem.baseHealth, playerSystem.health);
        // Wave (UI)
        UIWave.transform.GetChild(0).GetComponent<TMP_Text>().text = "Wave : " + enemySpawnerSystem.waveNumber;
        UIWave.transform.GetChild(1).GetComponent<TMP_Text>().text = "Enemies left : " + EnemySpawner.remainingEnemies;
        // Shards (UI)
        UIShards.transform.GetChild(0).GetComponent<TMP_Text>().text = ": " + playerSystem.iceShards.ToString();

    }

    public void onPlayerDeath()
    {
        Switch(overMenu) ;
    }

    public void onPlayerRespawn()
    {
        Switch(gameMenu);
    }
    

        

    public void Switch(GameObject to) {
        menuActif.SetActive(false);
        to.SetActive(true);

        menuActif = to;
    }


}
