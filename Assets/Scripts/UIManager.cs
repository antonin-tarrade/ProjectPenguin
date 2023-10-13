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

    // Player
    public Player player;

    // enemySpawner
    private EnemySpawner enemySpawner;

    // Menu
    public GameObject gameMenu;
    public GameObject pauseMenu;
    public GameObject overMenu;
    [HideInInspector]
    public GameObject difficultyMenu;

    // UIs
    public Health UIHealth;
    public TMP_Text UIShards;
    public TMP_Text UIWaveTxtWave;
    public TMP_Text UIWaveTXTEnemies;
    public TMP_Text UIScore;
    public Text scoreMort;

    // Default buttons (pour la borne)
    public Button pauseDefaultButton;
    public Button overDefaultButton;
    public Button difficultyDefaultButton;


    // Variables
    public GameObject menuActif;

    // Singleton
    public static UIManager instance;


    private void Awake() {

        Initialisation();

        //Initialisation Component
        UIHealth.InitHealthUI(player.baseHealth);

    }

    public void Initialisation() {

        Time.timeScale = 0f;
        
        menuActif = difficultyMenu;
        gameMenu.SetActive(false);
        pauseMenu.SetActive(false);
        overMenu.SetActive(false);
        difficultyMenu.SetActive(true);
        difficultyDefaultButton.Select();

        instance = this;
    }

    private void Start()
    {
        enemySpawner = EnemySpawner.instance;
        GameManager.instance.playerDeathEvent += onPlayerDeath;
        GameManager.instance.playerRespawnEvent += onPlayerRespawn;
    }

    private void Update() {

        // Menu Over
        if (menuActif == overMenu)
        {
            if (Input.GetKeyDown(KeyCode.R)) GameManager.instance.PlayerRespawn();

        }

        // Menu Pause
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.B)){
            if(!GameManager.instance.isPaused && !GameManager.instance.isShopOpen){
                GameManager.instance.Pause();
            }
            else if (GameManager.instance.isPaused && !GameManager.instance.isShopOpen){
                GameManager.instance.Unpause();
            }
        }
        if (GameManager.instance.isPaused) return;

        // Menu Game


        // Health (UI)
        UIHealth.UpdateHealthUI(player.baseHealth, player.health);
        // Wave (UI)
        UIWaveTxtWave.text = "Wave : " + (enemySpawner.waveNumber + 1);

        UIWaveTXTEnemies.text = "Enemies left : " + EnemySpawner.remainingEnemies;
        // Shards (UI)
        UIShards.text = ": " + player.iceShards.ToString();
        
        // Score (UI)
        UIScore.text = "Score : " + player.score.ToString();

    }

    public void onPlayerDeath()
    {
    	scoreMort.text = "Score : " + player.score.ToString();
        Switch(overMenu) ;
        overDefaultButton.Select();
    }

    public void onPlayerRespawn()
    {
        Switch(gameMenu);
        pauseDefaultButton.Select();
    }
    

    
    public void Switch(GameObject to) {
        menuActif.SetActive(false);
        //gameMenu.SetActive(true);
        to.SetActive(true);
	    Debug.Log("Switch : " + to.ToString() + " " + menuActif.ToString());
        menuActif = to;
    }


}
