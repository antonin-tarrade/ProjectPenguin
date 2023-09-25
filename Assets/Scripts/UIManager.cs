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
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject overMenu;
    public GameObject shopMenu;
    public GameObject difficultyMenu;

    
    // UIs
    public GameObject UIHealth;
    private Health UIHealthSystem;
    public TMP_Text UIShards;
    public TMP_Text UIWaveTxtWave;
    public TMP_Text UIWaveTXTEnemies;
    public TMP_Text UIScore;
    public Text scoreMort;

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
        
        menuActif = difficultyMenu;
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        overMenu.SetActive(false);
        shopMenu.SetActive(false);
        difficultyMenu.SetActive(true);

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
        UIWaveTxtWave.text = "Wave : " + (enemySpawnerSystem.waveNumber + 1);

        UIWaveTXTEnemies.text = "Enemies left : " + EnemySpawner.remainingEnemies;
        // Shards (UI)
        UIShards.text = ": " + playerSystem.iceShards.ToString();
        
        // Score (UI)
        UIScore.text = "Score : " + playerSystem.score.ToString();

    }

    public void onPlayerDeath()
    {
    	scoreMort.text = "Score : " + playerSystem.score.ToString();
        Switch(overMenu) ;
    }

    public void onPlayerRespawn()
    {
        Switch(gameMenu);
    }
    

        

    public void Switch(GameObject to) {
        menuActif.SetActive(false);
        //gameMenu.SetActive(true);
        to.SetActive(true);
	Debug.Log("Switch : " + to.ToString() + " " + menuActif.ToString());
        menuActif = to;
    }


}
